namespace Waillet.Blazor.Services;

public sealed class ApiResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }

    private ApiResult(bool isSuccess, T? value, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }

    public static ApiResult<T> Success(T value) => new(true, value, null);

    public static ApiResult<T> Fail(string errorMessage) => new(false, default, errorMessage);
}
