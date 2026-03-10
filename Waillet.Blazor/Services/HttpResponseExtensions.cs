using System.Net.Http.Json;

namespace Waillet.Blazor.Services;

internal static class HttpResponseExtensions
{
    public static async Task<ApiResult<T>> ToApiResultAsync<T>(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        if (response.IsSuccessStatusCode)
        {
            var value = await response.Content
                .ReadFromJsonAsync<T>(cancellationToken: cancellationToken);

            return value is null
                ? ApiResult<T>.Fail("Empty response from server.")
                : ApiResult<T>.Success(value);
        }

        var errorText = await response.Content.ReadAsStringAsync(cancellationToken);
        errorText = string.IsNullOrWhiteSpace(errorText)
            ? $"Request failed with status {(int)response.StatusCode}."
            : errorText.Trim().Trim('"');

        return ApiResult<T>.Fail(errorText);
    }
}
