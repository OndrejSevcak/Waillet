using WailletAPI.Dto;

namespace WailletAPI.Models.Auth;

public class AuthResult<T>
{
    public T ReturnObject { get; set; }
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}