using WailletAPI.Entities;

namespace WailletAPI.Services.Auth;

public interface IJwtTokenService
{
    /// <summary>
    /// Generate a JWT token for the authenticated user.
    /// </summary>
    /// <param name="user">The authenticated user</param>
    /// <returns>JWT token string</returns>
    string GenerateToken(User user);
}
