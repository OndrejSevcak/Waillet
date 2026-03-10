using System.Net.Http.Json;
using Waillet.Blazor.Models.Wallet;
using Waillet.Blazor.Services;

namespace Waillet.Blazor.Services.Wallet;

public class WalletApiClient : IWalletApiClient
{
    private readonly HttpClient _httpClient;

    public WalletApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResult<IReadOnlyList<AssetDto>>> GetSupportedAssetsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/wallet/assets", cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var assets = await response.Content.ReadFromJsonAsync<IReadOnlyList<AssetDto>>(cancellationToken: cancellationToken);
            if (assets is null)
            {
                return ApiResult<IReadOnlyList<AssetDto>>.Fail("Empty response from server.");
            }

            return ApiResult<IReadOnlyList<AssetDto>>.Success(assets);
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

        return ApiResult<IReadOnlyList<AssetDto>>.Fail(errorText);
    }
}
