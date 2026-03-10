using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Waillet.Blazor.Services.Auth;

public sealed class CustomAuthStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly IAuthStateService _authStateService;

    public CustomAuthStateProvider(IAuthStateService authStateService)
    {
        _authStateService = authStateService;
        _authStateService.AuthStateChanged += OnAuthStateChanged;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_authStateService.IsAuthenticated)
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        var user = _authStateService.CurrentUser;
        var claims = new List<Claim>();

        if (user is not null)
        {
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserKey.ToString()));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.Name, "User"));
        }

        var identity = new ClaimsIdentity(claims, "JwtAuth");
        var principal = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(principal));
    }

    private void OnAuthStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void Dispose()
    {
        _authStateService.AuthStateChanged -= OnAuthStateChanged;
    }
}
