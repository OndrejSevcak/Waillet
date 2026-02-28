namespace WailletAPI.Repository;

public interface ILedgerRepository
{
    Task<decimal> GetAccountBalanceAsync(long accKey);
}