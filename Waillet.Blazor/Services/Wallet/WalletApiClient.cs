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
        try
        {
            var response = await _httpClient.GetAsync("api/wallet/assets", cancellationToken);
            return await response.ToApiResultAsync<IReadOnlyList<AssetDto>>(cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            return ApiResult<IReadOnlyList<AssetDto>>.Fail($"Unable to reach the server: {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return ApiResult<IReadOnlyList<AssetDto>>.Fail("The request timed out. Please try again.");
        }
    }

    public async Task<ApiResult<AccountDto>> CreateWalletAccountAsync(string asset, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsync(
                $"api/wallet/accounts/create/{Uri.EscapeDataString(asset)}", null, cancellationToken);
            return await response.ToApiResultAsync<AccountDto>(cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            return ApiResult<AccountDto>.Fail($"Unable to reach the server: {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return ApiResult<AccountDto>.Fail("The request timed out. Please try again.");
        }
    }
}
