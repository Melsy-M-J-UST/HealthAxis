using System;
using System.Security.Cryptography;

namespace HealthAxis.Api.Helpers
{
    public static class PasswordHelper
    {
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider()) rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000))
                return Convert.ToBase64String(deriveBytes.GetBytes(32));
        }
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            return HashPassword(enteredPassword, storedSalt) == storedHash;
        }
    }
}
