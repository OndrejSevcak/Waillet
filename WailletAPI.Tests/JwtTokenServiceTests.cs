using System;
using Microsoft.Extensions.Options;
using Xunit;
using WailletAPI.Configuration;
using WailletAPI.Models;
using WailletAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace WailletAPI.Tests;

public class JwtTokenServiceTests
{
    private readonly JwtTokenService _service;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenServiceTests()
    {
        _jwtSettings = new JwtSettings
        {
            Secret = "THIS_IS_A_VERY_LONG_SECRET_KEY_FOR_JWT_TOKEN_GENERATION_MIN_256_BITS",
            Issuer = "WailletAPI",
            Audience = "WailletAPI",
            ExpirationMinutes = 60
        };
        _service = new JwtTokenService(Options.Create(_jwtSettings));
    }

    [Fact]
    public void GenerateToken_ValidUser_ReturnsToken()
    {
        // Arrange
        var user = new User
        {
            UserKey = 1,
            UserName = "testuser",
            NickName = "Test User",
            PasswordHash = new byte[32],
            PasswordSalt = new byte[32],
            Level = 1,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var token = _service.GenerateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void GenerateToken_ValidUser_TokenContainsCorrectClaims()
    {
        // Arrange
        var user = new User
        {
            UserKey = 123,
            UserName = "johndoe",
            NickName = "John Doe",
            PasswordHash = new byte[32],
            PasswordSalt = new byte[32],
            Level = 1,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var token = _service.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        Assert.Equal("123", jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
        Assert.Equal("johndoe", jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value);
        Assert.Equal("John Doe", jwtToken.Claims.First(c => c.Type == "nickname").Value);
        Assert.Equal(_jwtSettings.Issuer, jwtToken.Issuer);
        Assert.Equal(_jwtSettings.Audience, jwtToken.Audiences.First());
    }

    [Fact]
    public void GenerateToken_ValidUser_TokenHasExpiration()
    {
        // Arrange
        var user = new User
        {
            UserKey = 1,
            UserName = "testuser",
            NickName = "Test User",
            PasswordHash = new byte[32],
            PasswordSalt = new byte[32],
            Level = 1,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var token = _service.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
        Assert.True(jwtToken.ValidTo <= DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes + 1));
    }

    [Fact]
    public void GenerateToken_NullUser_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.GenerateToken(null!));
    }
}
