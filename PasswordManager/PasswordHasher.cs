
using System.Security.Cryptography;

namespace PasswordManager
{


    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 64;
        private const int Iterations = 10000;

        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return salt;
            }
        }

        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }
    }

}
