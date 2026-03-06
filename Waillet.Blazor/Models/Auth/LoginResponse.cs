namespace Waillet.Blazor.Models.Auth;

public class LoginResponse
{
    public required string AccessToken { get; set; }
    public required string TokenType { get; set; }
    public int ExpiresIn { get; set; }
}
