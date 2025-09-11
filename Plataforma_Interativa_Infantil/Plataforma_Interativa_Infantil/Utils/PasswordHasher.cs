using System;
using System.Security.Cryptography;

namespace backend.Utils;
public static class PasswordHasher {
    // PBKDF2 simples
    public static string Hash(string password) {
        var salt = new byte[16];
        using(var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);

        var result = new byte[49]; // 1 versão + 16 salt + 32 hash
        result[0] = 0x01;
        Buffer.BlockCopy(salt, 0, result, 1, 16);
        Buffer.BlockCopy(hash, 0, result, 17, 32);

        return Convert.ToBase64String(result);
    }

    public static bool Verify(string password, string stored) {
        try {
            var bytes = Convert.FromBase64String(stored);
            if (bytes.Length != 49 || bytes[0] != 0x01) return false;

            var salt = new byte[16];
            Buffer.BlockCopy(bytes, 1, salt, 0, 16);

            var hash = new byte[32];
            Buffer.BlockCopy(bytes, 17, hash, 0, 32);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            var test = pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(test, hash);
        } catch {
            return false;
        }
    }
}
