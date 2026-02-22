using Microsoft.AspNetCore.Mvc;
using WailletAPI.Common;
using WailletAPI.Dto;
using WailletAPI.Entities;
using WailletAPI.Services.Wallet;

namespace WailletAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IAccountService _accountService;

    public WalletController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("accounts")]
    public async Task<ActionResult<AccountDto>> CreateWalletAccount(CreateWalletDto request)
    {
        Result<Account> result = await _accountService.CreateWalletAccount(request.UserKey, request.Asset);
        
        if (result.IsSuccess)
        {
            AccountDto accountDto = new AccountDto
            {
                AccKey = result.Value.AccKey,
                UserKey = result.Value.UserKey,
                Asset = result.Value.Asset,
                CreatedAt = result.Value.CreatedAt
            };
            
            return Ok(accountDto);
        }

        return StatusCode((int)result.Error.Code, result.Error.Message);
    }
}