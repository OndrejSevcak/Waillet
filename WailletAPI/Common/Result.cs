namespace WailletAPI.Common;

public class Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }
    
    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Ok() => new Result(true, null);
    public static Result Fail(Error error) => new Result(false, error);
}