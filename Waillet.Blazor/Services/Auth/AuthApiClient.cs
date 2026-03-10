using System.Net.Http.Json;
using Waillet.Blazor.Models.Auth;
using Waillet.Blazor.Services;

namespace Waillet.Blazor.Services.Auth;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResult<UserDto>> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request, cancellationToken);
            return await response.ToApiResultAsync<UserDto>(cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            return ApiResult<UserDto>.Fail($"Unable to reach the server: {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return ApiResult<UserDto>.Fail("The request timed out. Please try again.");
        }
    }

    public async Task<ApiResult<LoginResponse>> LoginAsync(UserLoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request, cancellationToken);
            return await response.ToApiResultAsync<LoginResponse>(cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            return ApiResult<LoginResponse>.Fail($"Unable to reach the server: {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return ApiResult<LoginResponse>.Fail("The request timed out. Please try again.");
        }
    }
}
