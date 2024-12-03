namespace TimeTracker.Domain;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManger;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManger = userManager;
        _configuration = configuration;
    }

    public async Task<Result<RegisterResponse>> Handle(
        RegisterRequest request,
        CancellationToken cancellationToken
    )
    {
        if (request.Data.Password != request.Data.ConfirmPassword)
        {
            return Result.Fail<RegisterResponse>("Confirm Password doesn't match the password");
        }

        var userByEmail = await _userManger.FindByEmailAsync(request.Data.Email);
        if (userByEmail != null)
        {
            return Result.Fail<RegisterResponse>(
                $"User with the email {request.Data.Email} already exists"
            );
        }

        var identityUser = new ApplicationUser
        {
            Email = request.Data.Email,
            UserName = request.Data.Email,
            FirstName = request.Data.FirstName,
            LastName = request.Data.LastName,
        };

        var result = await _userManger.CreateAsync(identityUser, request.Data.Password);

        var userData = new UserManagerDto(
            new List<string>(),
            new Dictionary<string, string>(),
            null
        );

        return result.Succeeded
            ? Result.Ok(
                new RegisterResponse(userData)
                {
                    Message = "User created successfully",
                    IsSuccess = true,
                }
            )
            : Result.Fail<RegisterResponse>(
                result.Errors.Select(x => x.Description).Aggregate((a, b) => $"{a}, {b}")
            );
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userManger.FindByEmailAsync(request.Data.Email);

        if (user == null)
        {
            return Result.Fail<LoginResponse>(
                $"User with the email {request.Data.Email} doesn't exist"
            );
        }

        var result = await _userManger.CheckPasswordAsync(user, request.Data.Password);

        if (!result)
        {
            return Result.Fail<LoginResponse>("Password is incorrect");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, request.Data.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]!)
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["AuthSettings:Issuer"],
            audience: _configuration["AuthSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        var userData = new UserManagerDto(
            new List<string>(),
            claims.ToDictionary(c => c.Type, c => c.Value),
            token.ValidTo
        );

        return Result.Ok(
            new LoginResponse(
                new UserDto(Guid.Parse(user.Id), user.FirstName, user.LastName, user.Email!),
                new AccessTokenResultDto(tokenAsString, token.ValidTo)
            )
            {
                Message = "User logged in successfully",
                IsSuccess = true,
            }
        );
    }

    public async Task<Result<UserDto>> Handle(
        GetUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(request.Token);
        var user = await _userManger.FindByIdAsync(
            token.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value
        );
        if (user == null)
        {
            return Result.Fail<UserDto>("User not found");
        }

        return Result.Ok(
            new UserDto(Guid.Parse(user.Id), user.FirstName, user.LastName, user.Email!)
        );
    }
}
