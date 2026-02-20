using WailletAPI.Entities;

namespace WailletAPI.Services.Wallet;

public interface IAccountService
{
    Task<Account> CreateWalletAccount(long userKey, string asset);
}