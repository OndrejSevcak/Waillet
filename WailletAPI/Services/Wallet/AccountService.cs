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

    public AccountService(IUserRepository userRepository, IAccountRepository accountRepository)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }
    
    /// <summary>
    /// One wallet per user
    /// Multiple assets per wallet account
    /// Negative balance is not allowed
    /// </summary>
    public async Task<Account> CreateWalletAccount(long userKey, string asset)
    {
        var user = await _userRepository.GetByIdAsync(userKey);
        
        if (user is null)
        {
            throw new Exception("User not found");
        }
        
        var existingAccounts = await _accountRepository.GetAccountsByUserAsync(userKey);
        if (existingAccounts.Any())
        {
            throw new Exception("Account already exists");
        }
        
        var account = new Account
        {
            UserKey = userKey,
            Asset = asset,
        };
        
        return await _accountRepository.CreateAccountAsync(account);
    }
}