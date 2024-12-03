namespace TimeTracker.Contract.Dtos;

public record UserDto(Guid UserId, string FirstName, string LastName, string Email);
