using Waillet.Blazor.Models.Auth;

namespace Waillet.Blazor.Services.Auth;

public interface IAuthStateService
{
    string? AccessToken { get; }
    UserDto? CurrentUser { get; }
    bool IsAuthenticated { get; }
    void SetLoginResult(LoginResponse loginResponse, UserDto? user = null);
    void SetRegisteredUser(UserDto user);
    void ClearToken();
    event Action? AuthStateChanged;
}
