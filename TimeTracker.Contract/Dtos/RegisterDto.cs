namespace TimeTracker.Contract.Dtos;

public record RegisterDto(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword
);
