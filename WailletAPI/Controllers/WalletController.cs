using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPost("accounts/create/{asset}")]
    public async Task<ActionResult<AccountDto>> CreateWalletAccount([FromRoute] string asset)
    {
        var userKeyClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userKeyClaim, out var userKey))
        {
            return Unauthorized("Invalid user token");
        }

        Result<Account> result = await _accountService.CreateWalletAccount(userKey, asset);
        
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

        return StatusCode((int)result.Error!.Code, result.Error.Message);
    }

    [Authorize]
    [HttpGet("accounts/{accountKey:long}/balance")]
    public async Task<ActionResult<WalletBalanceDto>> GetWalletBalance([FromRoute] long accountKey)
    {
        var userKeyClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userKeyClaim, out var userKey))
        {
            return Unauthorized("Invalid user token");
        }

        var result = await _accountService.GetWalletBalanceAsync(userKey, accountKey);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return StatusCode((int)result.Error!.Code, result.Error.Message);
    }

    [Authorize]
    [HttpGet("accounts/{accountKey:long}/transactions")]
    public async Task<ActionResult<WalletTransactionHistoryPageDto>> GetAccountTransactionHistory(
        [FromRoute] long accountKey,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 100)
    {
        var userKeyClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userKeyClaim, out var userKey))
        {
            return Unauthorized("Invalid user token");
        }

        var result = await _accountService.GetAccountTransactionHistoryAsync(userKey, accountKey, page, pageSize);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return StatusCode((int)result.Error!.Code, result.Error.Message);
    }

    [HttpGet("assets")]
    public async Task<ActionResult<IReadOnlyList<AssetDto>>> GetSupportedAssets()
    {
        var result = await _accountService.GetSupportedAssetsAsync();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return StatusCode((int)result.Error!.Code, result.Error.Message);
    }
}