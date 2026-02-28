using WailletAPI.Common;
using WailletAPI.Entities;
using WailletAPI.Repository;
using WailletAPI.Services.Wallet;

namespace WailletAPI.Tests;

public class AccountServiceBalanceTests
{
    [Fact]
    public async Task GetWalletBalanceAsync_ExistingOwnedAccount_ReturnsAggregatedBalance()
    {
        var accountRepo = new FakeAccountRepository();
        accountRepo.Account = new Account { AccKey = 10, UserKey = 7, Asset = "BTC" };

        var ledgerRepo = new FakeLedgerRepository { Balance = 12.34m };

        var service = new AccountService(
            new FakeUserRepository(),
            accountRepo,
            new FakeAssetRepository(),
            ledgerRepo);

        var result = await service.GetWalletBalanceAsync(7, 10);

        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Value.AccKey);
        Assert.Equal("BTC", result.Value.Asset);
        Assert.Equal(12.34m, result.Value.Balance);
    }

    [Fact]
    public async Task GetWalletBalanceAsync_ExistingOwnedAccountWithoutEntries_ReturnsZeroBalance()
    {
        var accountRepo = new FakeAccountRepository();
        accountRepo.Account = new Account { AccKey = 11, UserKey = 5, Asset = "ETH" };

        var service = new AccountService(
            new FakeUserRepository(),
            accountRepo,
            new FakeAssetRepository(),
            new FakeLedgerRepository { Balance = 0m });

        var result = await service.GetWalletBalanceAsync(5, 11);

        Assert.True(result.IsSuccess);
        Assert.Equal(0m, result.Value.Balance);
    }

    [Fact]
    public async Task GetWalletBalanceAsync_MissingAccount_ReturnsNotFound()
    {
        var service = new AccountService(
            new FakeUserRepository(),
            new FakeAccountRepository(),
            new FakeAssetRepository(),
            new FakeLedgerRepository());

        var result = await service.GetWalletBalanceAsync(1, 999);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.NotFound, result.Error!.Code);
    }

    [Fact]
    public async Task GetWalletBalanceAsync_NotOwnedAccount_ReturnsForbidden()
    {
        var accountRepo = new FakeAccountRepository();
        accountRepo.Account = new Account { AccKey = 22, UserKey = 10, Asset = "BTC" };

        var service = new AccountService(
            new FakeUserRepository(),
            accountRepo,
            new FakeAssetRepository(),
            new FakeLedgerRepository { Balance = 50m });

        var result = await service.GetWalletBalanceAsync(9, 22);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.Forbidden, result.Error!.Code);
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        public Task<User> AddUserAsync(User user) => Task.FromResult(user);

        public Task<User?> GetByUserNameAsync(string userName) => Task.FromResult<User?>(null);

        public Task<User?> GetByIdAsync(long userKey) => Task.FromResult<User?>(null);
    }

    private sealed class FakeAccountRepository : IAccountRepository
    {
        public Account? Account { get; set; }

        public Task<IEnumerable<Account>> GetAccountsByUserAsync(long userKey)
        {
            IEnumerable<Account> accounts = Account is null ? [] : [Account];
            return Task.FromResult(accounts);
        }

        public Task<Account?> GetByAccKeyAsync(long accKey)
        {
            var account = Account is not null && Account.AccKey == accKey ? Account : null;
            return Task.FromResult(account);
        }

        public Task<Account> CreateAccountAsync(Account account) => Task.FromResult(account);
    }

    private sealed class FakeAssetRepository : IAssetRepository
    {
        public Task<IEnumerable<Asset>> GetAllAsync() => Task.FromResult<IEnumerable<Asset>>([]);
    }

    private sealed class FakeLedgerRepository : ILedgerRepository
    {
        public decimal Balance { get; set; }

        public Task<decimal> GetAccountBalanceAsync(long accKey) => Task.FromResult(Balance);
    }
}
