using WailletAPI.Common;
using WailletAPI.Entities;

namespace WailletAPI.Services.Wallet;

public interface IAccountService
{
    Task<Result<Account>> CreateWalletAccount(long userKey, string asset);
}