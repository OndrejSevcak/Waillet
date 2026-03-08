using WailletAPI.Common;
using WailletAPI.Dto;
using WailletAPI.Entities;
using WailletAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace WailletAPI.Services.Wallet;

/// <summary>
/// Wallet account management
/// Creates wallet accounts per user, maintain crypto assets, enforce account state
/// </summary>
public class AccountService : IAccountService
{
    private const int MaxTransactionsPageSize = 100;

    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAssetRepository _assetRepository;
    private readonly ILedgerRepository _ledgerRepository;

    public AccountService(
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        IAssetRepository assetRepository,
        ILedgerRepository ledgerRepository)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _assetRepository = assetRepository;
        _ledgerRepository = ledgerRepository;
    }
    
    /// <summary>
    /// One wallet per user per asset
    /// Negative balance is not allowed
    /// </summary>
    public async Task<Result<Account>> CreateWalletAccount(long userKey, string asset)
    {
        var normalizedAsset = asset.Trim().ToUpperInvariant();

        var user = await _userRepository.GetByIdAsync(userKey);
        if (user is null)
        {
            return Result<Account>.Fail(new Error(ErrorCode.BadRequest, "User not found"));
        }
        
        var availableAssets = await _assetRepository.GetAllAsync();
        if (!availableAssets.Any(a => a.Symbol.Equals(normalizedAsset, StringComparison.OrdinalIgnoreCase)))
        {
            return Result<Account>.Fail(new Error(ErrorCode.BadRequest, $"Asset '{asset}' is not supported"));
        }
        
        var existingAccounts = await _accountRepository.GetAccountsByUserAsync(userKey);
        if (existingAccounts.Any(a => a.Asset.Equals(normalizedAsset, StringComparison.OrdinalIgnoreCase)))
        {
            return Result<Account>.Fail(new Error(ErrorCode.Conflict, $"Wallet account for {asset} asset already exists"));
        }
        
        var account = new Account
        {
            UserKey = userKey,
            Asset = normalizedAsset,
        };

        try
        {
            var created = await _accountRepository.CreateAccountAsync(account);
            return Result<Account>.Ok(created);
        }
        catch (DbUpdateException)
        {
            return Result<Account>.Fail(new Error(ErrorCode.Conflict, $"Wallet account for {asset} asset already exists"));
        }
    }

    public async Task<Result<WalletBalanceDto>> GetWalletBalanceAsync(long userKey, long accKey)
    {
        var account = await _accountRepository.GetByAccKeyAsync(accKey);
        if (account is null)
        {
            return Result<WalletBalanceDto>.Fail(new Error(ErrorCode.NotFound, "Wallet account not found"));
        }

        if (account.UserKey != userKey)
        {
            return Result<WalletBalanceDto>.Fail(new Error(ErrorCode.Forbidden, "Access to wallet account is forbidden"));
        }

        var balance = await _ledgerRepository.GetAccountBalanceAsync(accKey);

        var walletBalance = new WalletBalanceDto
        {
            AccKey = account.AccKey,
            Asset = account.Asset,
            Balance = balance
        };

        return Result<WalletBalanceDto>.Ok(walletBalance);
    }

    public async Task<Result<WalletTransactionHistoryPageDto>> GetAccountTransactionHistoryAsync(long userKey, long accKey, int page, int pageSize)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = Math.Clamp(pageSize, 1, MaxTransactionsPageSize);

        var account = await _accountRepository.GetByAccKeyAsync(accKey);
        if (account is null)
        {
            return Result<WalletTransactionHistoryPageDto>.Fail(new Error(ErrorCode.NotFound, "Wallet account not found"));
        }

        if (account.UserKey != userKey)
        {
            return Result<WalletTransactionHistoryPageDto>.Fail(new Error(ErrorCode.Forbidden, "Access to wallet account is forbidden"));
        }

        var (ledgerEntries, totalCount) = await _ledgerRepository.GetAccountTransactionsPageAsync(accKey, normalizedPage, normalizedPageSize);

        var transactions = ledgerEntries
            .Select(entry => new WalletTransactionHistoryItemDto
            {
                Id = entry.Id,
                AccKey = entry.AccKey,
                Asset = entry.Asset,
                Amount = entry.Amount,
                Type = entry.Type,
                ReferenceId = entry.ReferenceId,
                ReferenceType = entry.ReferenceType,
                CreatedAt = entry.CreatedAt
            })
            .ToList();

        var availablePages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)normalizedPageSize);

        var pageResult = new WalletTransactionHistoryPageDto
        {
            TotalCount = totalCount,
            CurrentPage = normalizedPage,
            AvailablePages = availablePages,
            Transactions = transactions
        };

        return Result<WalletTransactionHistoryPageDto>.Ok(pageResult);
    }

    public async Task<Result<IReadOnlyList<AssetDto>>> GetSupportedAssetsAsync()
    {
        var assets = await _assetRepository.GetAllAsync();

        var assetDtos = assets
            .Select(asset => new AssetDto
            {
                Symbol = asset.Symbol,
                Name = asset.Name,
                Decimals = asset.Decimals
            })
            .ToList();

        return Result<IReadOnlyList<AssetDto>>.Ok(assetDtos);
    }
}