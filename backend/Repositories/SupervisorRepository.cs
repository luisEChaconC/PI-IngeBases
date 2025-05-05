using backend.Models;
using Microsoft.Data.SqlClient;

namespace backend.Repositories
{
    /// <summary>
    /// Repository for managing operations related to the Supervisors table.
    /// </summary>
    public class SupervisorRepository
    {
        private SqlConnection _connection; // SQL connection object
        private string _connectionString; // Connection string for the database

        /// <summary>
        /// Constructor to initialize the connection string and SQL connection.
        /// </summary>
        public SupervisorRepository()
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Inserts a new supervisor into the Supervisors table.
        /// </summary>
        /// <param name="supervisor">The supervisor model containing the data to insert.</param>
        public void CreateSupervisor(SupervisorModel supervisor)
        {
            // SQL query to insert a new supervisor
            var query = @"
                INSERT INTO Supervisors (Id)
                VALUES (@Id)";

            // Create a SQL command with the query and connection
            using (var command = new SqlCommand(query, _connection))
            {
                // Add parameters to the query to prevent SQL injection
                command.Parameters.AddWithValue("@Id", supervisor.Id);

                // Open the database connection
                _connection.Open();

                // Execute the query
                command.ExecuteNonQuery();

                // Close the database connection
                _connection.Close();
            }
        }
    }
}