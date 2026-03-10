using Waillet.Blazor.Models.Wallet;
using Waillet.Blazor.Services;

namespace Waillet.Blazor.Services.Wallet;

public interface IWalletApiClient
{
    Task<ApiResult<IReadOnlyList<AssetDto>>> GetSupportedAssetsAsync(CancellationToken cancellationToken = default);
    Task<ApiResult<AccountDto>> CreateWalletAccountAsync(string asset, CancellationToken cancellationToken = default);
}
