using System;

namespace WailletAPI.Services;

public interface IPasswordHashService
{
    /// <summary>
    /// Create a password hash and salt for storage.
    /// </summary>
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

    /// <summary>
    /// Verify supplied password against stored hash and salt.
    /// </summary>
    bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
}
