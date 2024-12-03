namespace TimeTracker.Contract.Requests.Users;

public class GetUserRequest : IRequest<Result<UserDto>>
{
    public GetUserRequest(string token)
    {
        Token = token;
    }

    public string Token { get; }
}
