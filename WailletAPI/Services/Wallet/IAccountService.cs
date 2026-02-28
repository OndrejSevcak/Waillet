using WailletAPI.Common;
using WailletAPI.Dto;
using WailletAPI.Entities;

namespace WailletAPI.Services.Wallet;

public interface IAccountService
{
    Task<Result<Account>> CreateWalletAccount(long userKey, string asset);
    Task<Result<WalletBalanceDto>> GetWalletBalanceAsync(long userKey, long accKey);
}