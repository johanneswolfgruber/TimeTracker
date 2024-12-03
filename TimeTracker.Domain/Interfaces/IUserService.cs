namespace TimeTracker.Domain;

public interface IUserService
    : IRequestHandler<RegisterRequest, Result<RegisterResponse>>,
        IRequestHandler<LoginRequest, Result<LoginResponse>>,
        IRequestHandler<GetUserRequest, Result<UserDto>> { }
