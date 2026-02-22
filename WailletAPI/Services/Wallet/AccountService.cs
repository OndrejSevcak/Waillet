using WailletAPI.Common;
using WailletAPI.Entities;
using WailletAPI.Repository;

namespace WailletAPI.Services.Wallet;

/// <summary>
/// Wallet account management
/// Creates wallet accounts per user, maintain crypto assets, enforce account state
/// </summary>
public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAssetRepository _assetRepository;

    public AccountService(IUserRepository userRepository, IAccountRepository accountRepository, IAssetRepository assetRepository)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _assetRepository = assetRepository;
    }
    
    /// <summary>
    /// One wallet per user per asset
    /// Negative balance is not allowed
    /// </summary>
    public async Task<Result<Account>> CreateWalletAccount(long userKey, string asset)
    {
        var user = await _userRepository.GetByIdAsync(userKey);
        if (user is null)
        {
            return Result<Account>.Fail(new Error(ErrorCode.BadRequest, "User not found"));
        }
        
        var availableAssets = await _assetRepository.GetAllAsync();
        if (!availableAssets.Any(a => a.Symbol.Equals(asset, StringComparison.OrdinalIgnoreCase)))
        {
            return Result<Account>.Fail(new Error(ErrorCode.BadRequest, $"Asset '{asset}' is not supported"));
        }
        
        var existingAccounts = await _accountRepository.GetAccountsByUserAsync(userKey);
        if (existingAccounts.Any(a => a.Asset == asset))
        {
            throw new Exception($"Wallet account for {asset} asset already exists");
        }
        
        var account = new Account
        {
            UserKey = userKey,
            Asset = asset,
        };
        
        var created = await _accountRepository.CreateAccountAsync(account);
        return Result<Account>.Ok(created);
    }
}