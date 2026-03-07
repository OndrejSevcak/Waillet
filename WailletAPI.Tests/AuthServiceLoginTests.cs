using Microsoft.Extensions.Options;
using WailletAPI.Common;
using WailletAPI.Configuration;
using WailletAPI.Dto;
using WailletAPI.Entities;
using WailletAPI.Repository;
using WailletAPI.Services;
using WailletAPI.Services.Auth;
using WailletAPI.Services.Auth.Impl;

namespace WailletAPI.Tests;

public class AuthServiceLoginTests
{
    [Fact]
    public async Task LoginUser_MissingUser_ReturnsUnauthorized()
    {
        var service = BuildService(new FakeUserRepository(), new FakePasswordHashService(), new FakeJwtTokenService());

        var result = await service.LoginUser(new UserLoginRequest { UserName = "missing@local", Password = "password" });

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.Unauthorized, result.Error!.Code);
        Assert.Equal("Invalid credentials", result.Error.Message);
    }

    [Fact]
    public async Task LoginUser_InvalidPassword_ReturnsUnauthorized()
    {
        var userRepo = new FakeUserRepository
        {
            User = new User
            {
                UserKey = 10,
                Email = "user@local",
                PasswordHash = new byte[32],
                PasswordSalt = new byte[32],
                CreatedAt = DateTime.UtcNow
            }
        };

        var service = BuildService(userRepo, new FakePasswordHashService { VerifyResult = false }, new FakeJwtTokenService());

        var result = await service.LoginUser(new UserLoginRequest { UserName = "user@local", Password = "wrong" });

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.Unauthorized, result.Error!.Code);
        Assert.Equal("Invalid credentials", result.Error.Message);
    }

    private static IAuthService BuildService(IUserRepository userRepository, IPasswordHashService passwordHashService, IJwtTokenService jwtTokenService)
    {
        var jwtSettings = Options.Create(new JwtSettings
        {
            Secret = "TEST_SECRET_32_CHARACTERS_LONG_123456",
            Issuer = "WailletAPI",
            Audience = "WailletAPI",
            ExpirationMinutes = 60
        });

        return new AuthService(userRepository, passwordHashService, jwtTokenService, jwtSettings);
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        public User? User { get; set; }

        public Task<User> AddUserAsync(User user) => Task.FromResult(user);

        public Task<User?> GetByUserNameAsync(string userName)
            => Task.FromResult(User is not null && User.Email == userName ? User : null);

        public Task<User?> GetByIdAsync(long userKey)
            => Task.FromResult(User is not null && User.UserKey == userKey ? User : null);
    }

    private sealed class FakePasswordHashService : IPasswordHashService
    {
        public bool VerifyResult { get; set; }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            passwordHash = Array.Empty<byte>();
            passwordSalt = Array.Empty<byte>();
        }

        public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt) => VerifyResult;
    }

    private sealed class FakeJwtTokenService : IJwtTokenService
    {
        public string GenerateToken(User user)
        {
            throw new InvalidOperationException("Token generation should not be called for login failures.");
        }
    }
}
