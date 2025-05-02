using backend.Models;
using Microsoft.Data.SqlClient;

namespace backend.Repositories
{
    public class UserRepository
    {
        private SqlConnection _connection; // SQL connection object
        private string _connectionString; // Connection string for the database

        // Constructor to initialize the connection string and SQL connection
        public UserRepository()
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            
            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);
        }

        // Method to retrieve a user by their email address
        public UserModel GetUserByEmail(string email)
        {
            // SQL query to select a user by email
            var query = "SELECT * FROM Users WHERE Email = @Email";

            // Create a SQL command with the query and connection
            using (var command = new SqlCommand(query, _connection))
            {
                // Add the email parameter to the query to prevent SQL injection
                command.Parameters.AddWithValue("@Email", email);

                // Open the database connection
                _connection.Open();

                // Execute the query and retrieve the results using a data reader
                using (var reader = command.ExecuteReader())
                {
                    // Check if a record was found
                    if (reader.Read())
                    {
                        // Map the data from the reader to a UserModel object
                        var user = new UserModel
                        {
                            Id = (int)reader["Id"], // Retrieve the user ID
                            Email = reader["Email"].ToString(), // Retrieve the email
                            Password = reader["Password"].ToString(), // Retrieve the password
                            IsAdmin = (bool)reader["IsAdmin"] // Retrieve the admin status
                        };

                        // Close the connection and return the user
                        _connection.Close();
                        return user;
                    }
                }

                // Close the connection if no user was found
                _connection.Close();
            }

            // Return null if no user was found
            return null;
        }
    }
}