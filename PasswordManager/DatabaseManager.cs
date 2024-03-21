
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

        public void InsertPassword(int userId, string url, string username, byte[] password, byte[] salt)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("InsertPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@URL", url);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Salt", salt);
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
                using (SqlCommand command = new SqlCommand("SELECT UserID, PasswordHash, Salt FROM Users WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userData = new UserData
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                PasswordHash = (byte[])reader["PasswordHash"],
                                Salt = (byte[])reader["Salt"]
                            };
                        }
                    }
                }
            }

            return userData;
        }

        // Other methods remain the same
    }

    public class UserData
    {
        public int UserID { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
    }



}
