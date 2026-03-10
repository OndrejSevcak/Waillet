using System.Net.Http.Headers;

namespace Waillet.Blazor.Services.Auth;

public sealed class AuthTokenHandler(IAuthStateService authState) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(authState.AccessToken))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", authState.AccessToken);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
