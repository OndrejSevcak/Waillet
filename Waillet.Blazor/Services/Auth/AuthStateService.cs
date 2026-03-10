using Waillet.Blazor.Models.Auth;

namespace Waillet.Blazor.Services.Auth;

public sealed class AuthStateService : IAuthStateService
{
    public string? AccessToken { get; private set; }
    public UserDto? CurrentUser { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken);

    public event Action? AuthStateChanged;

    public void SetLoginResult(LoginResponse loginResponse, UserDto? user = null)
    {
        AccessToken = loginResponse.AccessToken;
        if (user is not null)
        {
            CurrentUser = user;
        }
        AuthStateChanged?.Invoke();
    }

    public void SetRegisteredUser(UserDto user)
    {
        CurrentUser = user;
    }

    public void ClearToken()
    {
        AccessToken = null;
        CurrentUser = null;
        AuthStateChanged?.Invoke();
    }
}
