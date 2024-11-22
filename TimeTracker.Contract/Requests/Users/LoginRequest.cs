namespace TimeTracker.Contract.Requests.Users;

public class LoginRequest : IRequest<Result<LoginResponse>>
{
    public LoginRequest(LoginDto data)
    {
        Data = data;
    }

    public LoginDto Data { get; }
}

public class LoginResponse : ApiResponse
{
    public LoginResponse(UserDto user, AccessTokenResultDto token)
    {
        User = user;
        Token = token;
    }

    public UserDto User { get; }

    public AccessTokenResultDto Token { get; }
}
