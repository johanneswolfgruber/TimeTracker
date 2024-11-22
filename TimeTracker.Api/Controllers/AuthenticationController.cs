namespace TimeTracker.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public AuthenticationController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    [HttpGet("user")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<UserDto>))]
    [ProducesResponseType(401, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(404, Type = typeof(ApiErrorResponse))]
    public async Task<ActionResult<UserDto>> GetUser()
    {
        var token = await HttpContext.GetTokenAsync("Bearer", "access_token");
        if (token == null)
        {
            return Unauthorized(new ApiErrorResponse("Unauthorized", ["Unauthorized"]));
        }

        var result = await _mediator.Send(new GetUserRequest(token));

        return result.Success
            ? Ok(new ApiResponse<UserDto>(result.Value, "User retrieved successfully"))
            : NotFound(new ApiErrorResponse("Failed to get user", [result.Error]));
    }

    // /api/authentication/register
    [HttpPost("register")]
    [ProducesResponseType(200, Type = typeof(ApiResponse))]
    [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest model)
    {
        try
        {
            var result = await _mediator.Send(model);

            return result.Success
                ? Ok(new ApiResponse() { Message = "User created successfully!" })
                : BadRequest(
                    new ApiErrorResponse(
                        "Failed to create user",
                        result.Error.Split(",").Select(x => x.Trim()).ToArray()
                    )
                );
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse("Failed to create user", [ex.Message]));
        }
    }

    // /api/authentication/login
    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<LoginResponse>))]
    [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
    public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest model)
    {
        try
        {
            var result = await _mediator.Send(model);

            return result.Success
                ? Ok(
                    new ApiResponse<LoginResponse>(
                        result.Value,
                        "Access token retrieved successfully"
                    )
                )
                : BadRequest(new ApiErrorResponse("Failed to login", [result.Error]));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse("Failed to login", [ex.Message]));
        }
    }
}
