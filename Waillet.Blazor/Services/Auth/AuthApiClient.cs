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
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request, cancellationToken);
        return await ToResult<UserDto>(response, cancellationToken);
    }

    public async Task<ApiResult<LoginResponse>> LoginAsync(UserLoginRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request, cancellationToken);
        return await ToResult<LoginResponse>(response, cancellationToken);
    }

    private static async Task<ApiResult<T>> ToResult<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            var value = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
            if (value is null)
            {
                return ApiResult<T>.Fail("Empty response from server.");
            }

            return ApiResult<T>.Success(value);
        }

        var errorText = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(errorText))
        {
            errorText = $"Request failed with status {(int)response.StatusCode}.";
        }
        else
        {
            errorText = errorText.Trim().Trim('"');
        }

        return ApiResult<T>.Fail(errorText);
    }
}
