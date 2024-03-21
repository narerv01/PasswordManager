
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager
{


    public class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 64;
        private const int Iterations = 10000;


        public byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return salt;
            }
        }

        public byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }
         
        // Method to generate a password for a URL
        public string GeneratePasswordForUrl(UserData userData, string url)
        {
            // Derive key from password hash and email
            byte[] key = DeriveKey(userData.PasswordHash, userData.Email);

            // Generate a random password
            string generatedPassword = GenerateRandomPassword();

            // Encrypt the password using AES and retrieve the IV and encrypted password
            EncryptedData encryptedData = EncryptPasswordAES(generatedPassword, key);

            // Insert the encrypted password into the database along with other relevant information
            DatabaseManager db = new DatabaseManager();
            db.InsertPassword(userData, url, encryptedData.EncryptedPassword, encryptedData.IV);

            return generatedPassword; // Return the generated password
        }


        // Method to derive key from password hash and email
        private byte[] DeriveKey(byte[] passwordHash, string email)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(passwordHash, Encoding.UTF8.GetBytes(email), 10000))
            {
                return deriveBytes.GetBytes(32); // 256-bit key
            }
        }

        // Method to generate a random password
        private string GenerateRandomPassword(int length = 12)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            // Generate random characters for the password
            for (int i = 0; i < length; i++)
            {
                password.Append(allowedChars[random.Next(allowedChars.Length)]);
            }

            return password.ToString();
        }


        // Method to encrypt the password using AES
        public class EncryptedData
        {
            public byte[] IV { get; set; }
            public byte[] EncryptedPassword { get; set; }
        }

        // Method to encrypt the password using AES
        private EncryptedData EncryptPasswordAES(string password, byte[] key)
        {
            EncryptedData encryptedData = new EncryptedData();

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.GenerateIV();
                encryptedData.IV = aesAlg.IV; // Store the IV

                byte[] encryptedPassword;

                // Create an encryptor to perform the stream transform.
                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    // Create the streams used for encryption.
                    using (var msEncrypt = new System.IO.MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(password);
                            }
                            encryptedPassword = msEncrypt.ToArray();
                        }
                    }
                }

                encryptedData.EncryptedPassword = encryptedPassword; // Store the encrypted password
            }

            return encryptedData;
        }

        private string DecryptPasswordAES(byte[] encryptedPassword, byte[] key, byte[] iv, string email)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = DeriveKey(key, email);
                aesAlg.IV = iv;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (var msDecrypt = new System.IO.MemoryStream(encryptedPassword))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        public string DecryptPassword(byte[] encryptedPassword, byte[] passwordHash, byte[] iv, string email)
        {
            return DecryptPasswordAES(encryptedPassword, passwordHash, iv, email);
        }
         

    }

    // Class to store password data
    public class PasswordData
    {
        public string URL { get; set; }
        public byte[] EncryptedPassword { get; set; }
        public byte[] IV { get; set; }
    }

}
