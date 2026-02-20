using WailletAPI.Entities;

namespace WailletAPI.Repository;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetAccountsByUserAsync(long userKey);
    Task<Account> CreateAccountAsync(Account account);
}