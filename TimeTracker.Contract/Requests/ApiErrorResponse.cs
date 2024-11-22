namespace TimeTracker.Contract.Requests;

public class ApiErrorResponse
{
    public ApiErrorResponse() { }

    public ApiErrorResponse(string message)
    {
        Message = message;
        IsSuccess = false;
    }

    public ApiErrorResponse(string message, string[] errors)
    {
        IsSuccess = false;
        Message = message;
        Errors = errors;
    }

    public string Message { get; set; } = string.Empty;

    public string[] Errors { get; set; } = Array.Empty<string>();

    public bool IsSuccess { get; set; }
}
