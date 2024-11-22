namespace TimeTracker.Contract.Requests.Users;

public class RegisterRequest : IRequest<Result<RegisterResponse>>
{
    public RegisterRequest(RegisterDto data)
    {
        Data = data;
    }

    public RegisterDto Data { get; }
}

public class RegisterResponse : ApiResponse
{
    public RegisterResponse(UserManagerDto data)
    {
        Data = data;
    }

    public UserManagerDto Data { get; }
}
