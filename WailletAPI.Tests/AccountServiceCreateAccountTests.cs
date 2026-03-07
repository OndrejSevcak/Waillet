using Microsoft.EntityFrameworkCore;
using WailletAPI.Common;
using WailletAPI.Entities;
using WailletAPI.Repository;
using WailletAPI.Services.Wallet;

namespace WailletAPI.Tests;

public class AccountServiceCreateAccountTests
{
    [Fact]
    public async Task CreateWalletAccount_ValidRequest_NormalizesAssetSymbol()
    {
        var userRepo = new FakeUserRepository { User = new User { UserKey = 1, Email = "user@local", CreatedAt = DateTime.UtcNow } };
        var accountRepo = new FakeAccountRepository();
        var assetRepo = new FakeAssetRepository
        {
            Assets = [new Asset { Symbol = "BTC", IsActive = true }]
        };

        var service = new AccountService(userRepo, accountRepo, assetRepo, new FakeLedgerRepository());

        var result = await service.CreateWalletAccount(1, "  btc ");

        Assert.True(result.IsSuccess);
        Assert.Equal("BTC", result.Value.Asset);
    }

    [Fact]
    public async Task CreateWalletAccount_DbUpdateException_ReturnsConflict()
    {
        var userRepo = new FakeUserRepository { User = new User { UserKey = 2, Email = "race@local", CreatedAt = DateTime.UtcNow } };
        var accountRepo = new FakeAccountRepository { ThrowOnCreate = true };
        var assetRepo = new FakeAssetRepository
        {
            Assets = [new Asset { Symbol = "ETH", IsActive = true }]
        };

        var service = new AccountService(userRepo, accountRepo, assetRepo, new FakeLedgerRepository());

        var result = await service.CreateWalletAccount(2, "ETH");

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.Conflict, result.Error!.Code);
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        public User? User { get; set; }

        public Task<User> AddUserAsync(User user) => Task.FromResult(user);

        public Task<User?> GetByUserNameAsync(string userName) => Task.FromResult<User?>(null);

        public Task<User?> GetByIdAsync(long userKey)
        {
            var user = User is not null && User.UserKey == userKey ? User : null;
            return Task.FromResult(user);
        }
    }

    private sealed class FakeAccountRepository : IAccountRepository
    {
        public bool ThrowOnCreate { get; set; }
        public List<Account> Accounts { get; } = [];

        public Task<IEnumerable<Account>> GetAccountsByUserAsync(long userKey)
        {
            IEnumerable<Account> accounts = Accounts.Where(a => a.UserKey == userKey).ToList();
            return Task.FromResult(accounts);
        }

        public Task<Account?> GetByAccKeyAsync(long accKey)
        {
            var account = Accounts.FirstOrDefault(a => a.AccKey == accKey);
            return Task.FromResult(account);
        }

        public Task<Account> CreateAccountAsync(Account account)
        {
            if (ThrowOnCreate)
            {
                throw new DbUpdateException("Unique constraint violation.");
            }

            Accounts.Add(account);
            return Task.FromResult(account);
        }
    }

    private sealed class FakeAssetRepository : IAssetRepository
    {
        public List<Asset> Assets { get; set; } = [];

        public Task<IEnumerable<Asset>> GetAllAsync() => Task.FromResult<IEnumerable<Asset>>(Assets);
    }

    private sealed class FakeLedgerRepository : ILedgerRepository
    {
        public Task<decimal> GetAccountBalanceAsync(long accKey) => Task.FromResult(0m);

        public Task<(IReadOnlyList<Ledger> Transactions, int TotalCount)> GetAccountTransactionsPageAsync(long accKey, int page, int pageSize)
            => Task.FromResult(((IReadOnlyList<Ledger>)Array.Empty<Ledger>(), 0));
    }
}
