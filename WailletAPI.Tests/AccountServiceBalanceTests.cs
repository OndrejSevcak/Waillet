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

    [Fact]
    public async Task GetAccountTransactionHistoryAsync_ExistingOwnedAccount_ReturnsTransactionHistory()
    {
        var accountRepo = new FakeAccountRepository
        {
            Account = new Account { AccKey = 42, UserKey = 5, Asset = "BTC" }
        };

        var ledgerRepo = new FakeLedgerRepository
        {
            Transactions =
            [
                new Ledger
                {
                    Id = 1001,
                    AccKey = 42,
                    Asset = "BTC",
                    Amount = 0.5m,
                    Type = "Deposit",
                    ReferenceId = 901,
                    ReferenceType = "Deposit",
                    CreatedAt = new DateTime(2026, 1, 10, 8, 30, 0, DateTimeKind.Utc)
                },
                new Ledger
                {
                    Id = 1002,
                    AccKey = 42,
                    Asset = "BTC",
                    Amount = -0.1m,
                    Type = "Withdrawal",
                    ReferenceId = 902,
                    ReferenceType = "Withdrawal",
                    CreatedAt = new DateTime(2026, 1, 11, 9, 15, 0, DateTimeKind.Utc)
                }
            ]
        };

        var service = new AccountService(
            new FakeUserRepository(),
            accountRepo,
            new FakeAssetRepository(),
            ledgerRepo);

        var result = await service.GetAccountTransactionHistoryAsync(5, 42);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal(1001, result.Value[0].Id);
        Assert.Equal("Deposit", result.Value[0].Type);
        Assert.Equal(1002, result.Value[1].Id);
        Assert.Equal("Withdrawal", result.Value[1].Type);
    }

    [Fact]
    public async Task GetAccountTransactionHistoryAsync_MissingAccount_ReturnsNotFound()
    {
        var service = new AccountService(
            new FakeUserRepository(),
            new FakeAccountRepository(),
            new FakeAssetRepository(),
            new FakeLedgerRepository());

        var result = await service.GetAccountTransactionHistoryAsync(1, 999);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.NotFound, result.Error!.Code);
    }

    [Fact]
    public async Task GetAccountTransactionHistoryAsync_NotOwnedAccount_ReturnsForbidden()
    {
        var accountRepo = new FakeAccountRepository
        {
            Account = new Account { AccKey = 77, UserKey = 11, Asset = "ETH" }
        };

        var service = new AccountService(
            new FakeUserRepository(),
            accountRepo,
            new FakeAssetRepository(),
            new FakeLedgerRepository());

        var result = await service.GetAccountTransactionHistoryAsync(3, 77);

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
        public IReadOnlyList<Ledger> Transactions { get; set; } = [];

        public Task<decimal> GetAccountBalanceAsync(long accKey) => Task.FromResult(Balance);

        public Task<IReadOnlyList<Ledger>> GetAccountTransactionsAsync(long accKey) => Task.FromResult(Transactions);
    }
}
