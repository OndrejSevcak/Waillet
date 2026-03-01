using Microsoft.AspNetCore.Mvc;
using WailletAPI.Dto;
using WailletAPI.Services;

namespace WailletAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest req)
    {
        var result = await _authService.RegisterUser(req);
        
        if (!result.IsSuccess)
            return StatusCode((int)result.Error!.Code, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest req)
    {
        var result = await _authService.LoginUser(req);
        
        if (!result.IsSuccess)
            return StatusCode((int)result.Error!.Code, result.Error.Message);

        return Ok(result.Value);
    }
}
