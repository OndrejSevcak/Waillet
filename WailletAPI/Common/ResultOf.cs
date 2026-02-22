namespace WailletAPI.Common;

public class Result<T> : Result
{
    public T Value { get; }
    
    protected Result(T value) : base(true, null)
    {
        Value = value;
    }
    
    protected Result(Error error) : base(false, error)
    {
        Value = default!;
    }
    
    public static Result<T> Ok(T value) => new Result<T>(value);
    public new static Result<T> Fail(Error error) => new Result<T>(error);
    
}