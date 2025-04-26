using backend.Models;
using Microsoft.Data.SqlClient;

namespace backend.Handlers
{
    public class UserHandler
    {
        private SqlConnection _connection;
        private string _connectionString;
        public UserHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        public UserModel GetUserByEmail(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";

            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = new UserModel
                        {
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString()
                        };
                        _connection.Close();
                        return user;
                    }
                }
                _connection.Close();
            }

            return null;
        }
    }
}
