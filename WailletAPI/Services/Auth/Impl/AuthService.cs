using Microsoft.Extensions.Options;
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


    public AuthService(
        IUserRepository userRepository, 
        IPasswordHashService passwordService, 
        IJwtTokenService jwtTokenService, 
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult<UserDto>> RegisterUser(RegisterUserRequest req)
    {
        var existing = await _userRepository.GetByUserNameAsync(req.Email);
        if (existing != null)
            return new AuthResult<UserDto>()
            {
                Success = false,
                StatusCode = 409,
                Message = "UserName already taken"
            };

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

        return new AuthResult<UserDto>()
        {
            Success = true,
            StatusCode = 201,
            ReturnObject = new UserDto()
            {
                UserKey = created.UserKey,
                UserName = created.Email,
                CreatedAt = created.CreatedAt
            }
        };
    }
    
    public async Task<AuthResult<LoginResponse>> LoginUser(UserLoginRequest req)
    {
        var user = await _userRepository.GetByUserNameAsync(req.UserName);
        if (user == null)
            return new AuthResult<LoginResponse>()
            {
                Success = false,
                StatusCode = 401,
                Message = "Invalid credentials"
            };

        var valid = _passwordService.VerifyPassword(req.Password, user.PasswordHash, user.PasswordSalt);
        if (!valid)
            return new AuthResult<LoginResponse>()
            {
                Success = false,
                StatusCode = 401,
                Message = "Invalid credentials"
            };

        var token = _jwtTokenService.GenerateToken(user);

        return new AuthResult<LoginResponse>()
        {
            Success = true,
            StatusCode = 200,
            ReturnObject = new LoginResponse(
                AccessToken: token,
                TokenType: "Bearer",
                ExpiresIn: 3600 // This should ideally come from JwtSettings
            )
        };
    }
}