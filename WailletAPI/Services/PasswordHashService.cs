using System;
using System.Security.Cryptography;

namespace WailletAPI.Services;

public class PasswordHashService : IPasswordHashService
{
    private const int SaltSize = 32; // 32 bytes
    private const int HashSize = 32; // 32 bytes
    private const int Iterations = 100_000; // PBKDF2 iterations

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        passwordSalt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(passwordSalt);
        }

        using (var deriveBytes = new Rfc2898DeriveBytes(password, passwordSalt, Iterations, HashAlgorithmName.SHA256))
        {
            passwordHash = deriveBytes.GetBytes(HashSize);
        }
    }

    public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        if (string.IsNullOrEmpty(password)) return false;
        if (storedHash is not { Length: HashSize }) return false;
        if (storedSalt is not { Length: SaltSize }) return false;

        byte[] computedHash;
        using (var deriveBytes = new Rfc2898DeriveBytes(password, storedSalt, Iterations, HashAlgorithmName.SHA256))
        {
            computedHash = deriveBytes.GetBytes(HashSize);
        }

        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }
}
