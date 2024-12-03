namespace TimeTracker.Common;

public class Result
{
    protected Result(bool success, string error)
    {
        if (success && !string.IsNullOrEmpty(error) || !success && string.IsNullOrEmpty(error))
        {
            throw new ArgumentException("Success and error are mutually exclusive");
        }

        Success = success;
        Error = error;
    }

    public bool Success { get; private set; }
    public string Error { get; private set; }

    public bool Failure
    {
        get { return !Success; }
    }

    public static Result Fail(string message)
    {
        return new Result(false, message);
    }

    public static Result<T> Fail<T>(string message)
    {
        return new Result<T>(default(T)!, false, message);
    }

    public static Result Ok()
    {
        return new Result(true, String.Empty);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true, String.Empty);
    }

    public static Result Combine(params Result[] results)
    {
        foreach (Result result in results)
        {
            if (result.Failure)
            {
                return result;
            }
        }

        return Ok();
    }
}

public class Result<T> : Result
{
    private T _value = default(T)!;

    public T Value
    {
        get
        {
            if (!Success)
            {
                throw new InvalidOperationException("Cannot get value from failed result");
            }

            return _value;
        }
        private set { _value = value; }
    }

    protected internal Result(T value, bool success, string error)
        : base(success, error)
    {
        if (success && value == null)
        {
            throw new ArgumentException("Success and value are mutually exclusive");
        }

        Value = value;
    }
}
