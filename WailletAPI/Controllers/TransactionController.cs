using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WailletAPI.Dto;
using WailletAPI.Services;

namespace WailletAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _service;

    public TransactionController(TransactionService service)
    {
        _service = service;
    }

    [HttpPost("exchange")]
    [Authorize]
    public async Task<IActionResult> Exchange([FromBody] CreateTransactionRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _service.ExchangeAsync(req, User);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("One or both accounts not found");
        }
        catch (UnauthorizedAccessException u)
        {
            return Unauthorized(u.Message);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
