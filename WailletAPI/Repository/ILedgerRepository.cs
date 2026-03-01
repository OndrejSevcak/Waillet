using WailletAPI.Entities;

namespace WailletAPI.Repository;

public interface ILedgerRepository
{
    Task<decimal> GetAccountBalanceAsync(long accKey);
    Task<(IReadOnlyList<Ledger> Transactions, int TotalCount)> GetAccountTransactionsPageAsync(long accKey, int page, int pageSize);
}