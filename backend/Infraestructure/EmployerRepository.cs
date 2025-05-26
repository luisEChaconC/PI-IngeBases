using backend.Domain;
using Microsoft.Data.SqlClient;

namespace backend.Infraestructure
{
    /// <summary>
    /// Repository for managing operations related to the Employers table.
    /// </summary>
    public class EmployerRepository
    {
        private SqlConnection _connection; // SQL connection object
        private string _connectionString; // Connection string for the database

        /// <summary>
        /// Constructor to initialize the connection string and SQL connection.
        /// </summary>
        public EmployerRepository()
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Inserts a new employer into the Employers table.
        /// </summary>
        /// <param name="employer">The employer model containing the data to insert.</param>
        public void CreateEmployer(EmployerModel employer)
        {
            // SQL query to insert a new employer
            var query = @"
                INSERT INTO Employers (Id, CompanyId)
                VALUES (@Id, @CompanyId)";

            // Create a SQL command with the query and connection
            using (var command = new SqlCommand(query, _connection))
            {
                // Add parameters to the query to prevent SQL injection
                command.Parameters.AddWithValue("@Id", employer.Id);
                command.Parameters.AddWithValue("@CompanyId", employer.CompanyId);

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