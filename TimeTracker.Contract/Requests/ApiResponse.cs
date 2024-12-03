namespace TimeTracker.Contract.Requests;

public class ApiBaseResponse
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
}

public class ApiResponse : ApiBaseResponse
{
    public ApiResponse(string message)
    {
        IsSuccess = true;
        Message = message;
    }

    public ApiResponse()
    {
        IsSuccess = true;
    }
}

public class ApiResponse<T> : ApiBaseResponse
{
    public ApiResponse(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    public ApiResponse(T value, string message)
    {
        IsSuccess = true;
        Value = value;
        Message = message;
    }

    public T Value { get; set; }
}
