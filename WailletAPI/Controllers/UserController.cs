using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WailletAPI.Configuration;
using WailletAPI.Dto;
using WailletAPI.Models;
using WailletAPI.Repository;
using WailletAPI.Services;

namespace WailletAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserRepository _repository;
    private readonly IPasswordHashService _passwordService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;

    public UserController(UserRepository repository, IPasswordHashService passwordService, IJwtTokenService jwtTokenService, IOptions<JwtSettings> jwtSettings)
    {
        _repository = repository;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserName) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest("UserName and Password are required");

        var existing = await _repository.GetByUserNameAsync(req.UserName);
        if (existing != null)
            return Conflict("UserName already taken");

        _passwordService.CreatePasswordHash(req.Password, out var hash, out var salt);

        var user = new User
        {
            UserName = req.UserName,
            PasswordHash = hash,
            PasswordSalt = salt,
            NickName = req.NickName,
            Level = 1,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddUserAsync(user);

        return Ok(new { user.UserKey, user.UserName, user.NickName });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest req)
    {
        var user = await _repository.GetByUserNameAsync(req.UserName);
        if (user == null)
            return Unauthorized();

        var valid = _passwordService.VerifyPassword(req.Password, user.PasswordHash, user.PasswordSalt);
        if (!valid)
            return Unauthorized();

        var token = _jwtTokenService.GenerateToken(user);
        
        return Ok(new LoginResponse(
            AccessToken: token,
            TokenType: "Bearer",
            ExpiresIn: _jwtSettings.ExpirationMinutes * 60
        ));
    }
}
