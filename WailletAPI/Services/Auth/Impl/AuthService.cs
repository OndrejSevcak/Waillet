using Microsoft.Extensions.Options;
using WailletAPI.Common;
using WailletAPI.Configuration;
using WailletAPI.Dto;
using WailletAPI.Entities;
using WailletAPI.Models.Auth;
using WailletAPI.Repository;

namespace WailletAPI.Services.Auth.Impl;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IOptions<JwtSettings> _jwtSettings;

    public AuthService(
        IUserRepository userRepository, 
        IPasswordHashService passwordService, 
        IJwtTokenService jwtTokenService, 
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings;
    }

    public async Task<Result<UserDto>> RegisterUser(RegisterUserRequest req)
    {
        var existing = await _userRepository.GetByUserNameAsync(req.Email);
        if (existing != null)
            return Result<UserDto>.Fail(new Error(ErrorCode.BadRequest, "A user with this email already exists."));

        _passwordService.CreatePasswordHash(req.Password, out var hash, out var salt);

        var user = new User
        {
            Email = req.Email,
            PasswordHash = hash,
            PasswordSalt = salt,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var created = await _userRepository.AddUserAsync(user);

        return Result<UserDto>.Ok(new UserDto()
        {
            UserKey = created.UserKey,
            UserName = created.Email,
            CreatedAt = created.CreatedAt
        });
    }
    
    public async Task<Result<LoginResponse>> LoginUser(UserLoginRequest req)
    {
        var user = await _userRepository.GetByUserNameAsync(req.UserName);
        if (user == null)
            return Result<LoginResponse>.Fail(new Error(ErrorCode.NotFound, "User not found."));

        var valid = _passwordService.VerifyPassword(req.Password, user.PasswordHash, user.PasswordSalt);
        if (!valid)
            return Result<LoginResponse>.Fail(new Error(ErrorCode.Unauthorized, "Invalid credentials"));

        var token = _jwtTokenService.GenerateToken(user);

        return Result<LoginResponse>.Ok(new LoginResponse
        {
            AccessToken = token,
            TokenType = "Bearer",
            ExpiresIn = _jwtSettings.Value.ExpirationMinutes
        });
    }
}