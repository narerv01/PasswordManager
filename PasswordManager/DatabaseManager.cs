
using System.Data;
using System.Data.SqlClient;

namespace PasswordManager
{


    public class DatabaseManager
    {
        private string connectionString = $"Server=SB-R90Q2SJA\\MSSQLSERVER01;Database=XML;Integrated Security=True;";

        public DatabaseManager()
        {
        }

        public void InsertUser(string email, byte[] passwordHash, byte[] salt)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("InsertUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    command.Parameters.AddWithValue("@Salt", salt);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertPassword(UserData userData, string url, byte[] encryptedPassword, byte[] iv)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("InsertPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userData.UserID);
                    command.Parameters.AddWithValue("@URL", url);
                    command.Parameters.AddWithValue("@EncryptedPassword", encryptedPassword);
                    command.Parameters.AddWithValue("@IV", iv);
                    command.ExecuteNonQuery();
                }
            }
        }




        public UserData GetUserByEmail(string email)
        {
            UserData userData = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetUserByEmail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userData = new UserData
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                PasswordHash = (byte[])reader["PasswordHash"],
                                Salt = (byte[])reader["Salt"]
                            };
                        }
                    }
                }
            }

            return userData;
        }

        public List<PasswordData> GetPasswordsForUser(int userId)
        {
            List<PasswordData> passwords = new List<PasswordData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetPasswordsForUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PasswordData passwordData = new PasswordData
                            {
                                URL = reader.GetString(0),
                                EncryptedPassword = (byte[])reader["EncryptedPassword"],
                                IV = (byte[])reader["IV"]
                            };
                            passwords.Add(passwordData);
                        }
                    }
                }
            }

            return passwords;
        }
    }
    public class UserData
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
    }

}






 
