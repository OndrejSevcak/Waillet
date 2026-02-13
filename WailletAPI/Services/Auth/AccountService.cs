using WailletAPI.Dto;
using WailletAPI.Models;
using WailletAPI.Repository;

namespace WailletAPI.Services;

public class AccountService
{
    private readonly AccountRepository _accountRepo;
    private readonly UserRepository _userRepo;
    private readonly CryptoCurrencyRepository _cryptoRepo;

    public AccountService(AccountRepository accountRepo, UserRepository userRepo, CryptoCurrencyRepository cryptoRepo)
    {
        _accountRepo = accountRepo;
        _userRepo = userRepo;
        _cryptoRepo = cryptoRepo;
    }

    public async Task<AccountDto> CreateAccountAsync(CreateAccountRequest req)
    {
        // Ensure user exists
        var user = await _userRepo.GetByIdAsync(req.UserKey);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        // If this is a crypto account, the CurrencyCode must be one of supported cryptos (in DB)
        if (req.CryptoFlag)
        {
            if (string.IsNullOrWhiteSpace(req.CurrencyCode))
                throw new InvalidOperationException("CurrencyCode must be provided for crypto accounts");

            var exists = await _cryptoRepo.ExistsAsync(req.CurrencyCode);
            if (!exists)
                throw new InvalidOperationException($"Unsupported crypto currency: {req.CurrencyCode}");

            // normalize code to upper-case
            req.CurrencyCode = req.CurrencyCode.Trim().ToUpperInvariant();
        }

        var account = new Account
        {
            UserKey = req.UserKey,
            CryptoFlag = req.CryptoFlag,
            CurrencyCode = req.CurrencyCode,
            Balance = req.InitialBalance,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        await _accountRepo.AddAccount(account);

        var dto = MapToDto(account);
        
        return dto;
    }

    private AccountDto MapToDto(Account account)
    {
        return new AccountDto
        {
            AccKey = account.AccKey,
            CryptoFlag = account.CryptoFlag,
            CurrencyCode = account.CurrencyCode,
            Balance = account.Balance,
            Active = account.Active,
            CreatedAt = account.CreatedAt
        };
    }
}
