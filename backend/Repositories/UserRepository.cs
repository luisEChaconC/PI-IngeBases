using backend.Models;
using Microsoft.Data.SqlClient;

namespace backend.Repositories
{
    /// <summary>
    /// Repository for managing operations related to the Users table.
    /// </summary>
    public class UserRepository
    {
        private SqlConnection _connection; // SQL connection object
        private string _connectionString; // Connection string for the database

        /// <summary>
        /// Constructor to initialize the connection string and SQL connection.
        /// </summary>
        public UserRepository()
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Inserts a new user into the Users table.
        /// </summary>
        /// <param name="user">The user model containing the data to insert.</param>
        /// <returns>The database-generated ID of the new user.</returns>
        public string CreateUser(UserModel user)
        {
            string userId = string.Empty;

            // SQL query to insert a new user and return the generated ID
            var query = @"
                INSERT INTO Users (Email, Password, IsAdmin)
                OUTPUT INSERTED.Id
                VALUES (@Email, @Password, @IsAdmin)";

            // Create a SQL command with the query and connection
            using (var command = new SqlCommand(query, _connection))
            {
                // Add parameters to the query to prevent SQL injection
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);

                // Open the database connection
                _connection.Open();

                // Execute the query and get the inserted ID
                userId = command.ExecuteScalar()?.ToString() ?? string.Empty;

                // Close the database connection
                _connection.Close();
            }

            return userId;
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>A UserModel object if the user is found; otherwise, null.</returns>
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
                            Id = reader["Id"].ToString(), // Retrieve the user ID as string
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

        /// <summary>
        /// Retrieves detailed user information by email, including roles and related data.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>An object containing detailed user information, or null if not found.</returns>
        public object GetUserInformationByEmail(string email)
        {
            // SQL query to retrieve user information with multiple joins
            var query = @"
                SELECT 
                    u.Id AS IdUsuario,
                    u.Email,
                    u.IsAdmin,
                    p.Id AS IdPerson,
                    np.Id AS IdNaturalPerson,
                    np.FirstName,
                    np.FirstSurname,
                    np.SecondSurname,
                    COALESCE(e.CompanyId, em.CompanyId) AS CompanyId, -- Get CompanyId from Employees or Employers
                    CASE 
                        WHEN s.Id IS NOT NULL THEN 'Supervisor'
                        WHEN pm.Id IS NOT NULL THEN 'Payroll Manager'
                        WHEN em.Id IS NOT NULL THEN 'Employer'
                        ELSE 'Collaborator'
                    END AS Position
                FROM Users u
                LEFT JOIN NaturalPersons np ON u.Id = np.UserId
                LEFT JOIN Persons p ON np.Id = p.Id
                LEFT JOIN Employees e ON np.Id = e.Id
                LEFT JOIN Supervisors s ON e.Id = s.Id
                LEFT JOIN PayrollManagers pm ON e.Id = pm.Id
                LEFT JOIN Employers em ON np.Id = em.Id
                WHERE u.Email = @Email";

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
                        // Map the data from the reader to an anonymous object
                        var userInfo = new
                        {
                            IdUsuario = reader["IdUsuario"].ToString(),
                            Email = reader["Email"].ToString(),
                            IsAdmin = (bool)reader["IsAdmin"],
                            IdPerson = reader["IdPerson"].ToString(),
                            IdNaturalPerson = reader["IdNaturalPerson"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            FirstSurname = reader["FirstSurname"].ToString(),
                            SecondSurname = reader["SecondSurname"].ToString(),
                            CompanyId = reader["CompanyId"] != DBNull.Value ? reader["CompanyId"].ToString() : null,
                            Position = reader["Position"].ToString()
                        };

                        // Close the connection and return the user information
                        _connection.Close();
                        return userInfo;
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