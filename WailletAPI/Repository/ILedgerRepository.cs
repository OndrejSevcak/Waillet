using WailletAPI.Entities;

namespace WailletAPI.Repository;

public interface ILedgerRepository
{
    Task<decimal> GetAccountBalanceAsync(long accKey);
    Task<IReadOnlyList<Ledger>> GetAccountTransactionsAsync(long accKey);
}