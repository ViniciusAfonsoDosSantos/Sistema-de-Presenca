using System;
using System.Security.Cryptography;

namespace TrabalhoInterdisciplinar.Helpers
{
    public static class PasswordHasher
    {
        private static int nIterations = 10101;
        private static int nHash = 70;
        private static int salt = 70;
        public static string GenerateSalt(int nSalt)
        {
            var saltBytes = new byte[nSalt];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password)
        {
            var saltBytes = Convert.FromBase64String(GenerateSalt(salt));

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
            }
        }
    }
}
