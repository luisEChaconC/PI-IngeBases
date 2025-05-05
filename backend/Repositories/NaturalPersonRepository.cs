using backend.Models;
using Microsoft.Data.SqlClient;

namespace backend.Repositories
{
    /// <summary>
    /// Repository for managing operations related to the NaturalPersons table.
    /// </summary>
    public class NaturalPersonRepository
    {
        private SqlConnection _connection; // SQL connection object
        private string _connectionString; // Connection string for the database

        /// <summary>
        /// Constructor to initialize the connection string and SQL connection.
        /// </summary>
        public NaturalPersonRepository()
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Inserts a new natural person into the NaturalPersons table.
        /// </summary>
        /// <param name="naturalPerson">The natural person model containing the data to insert.</param>
        public void CreateNaturalPerson(NaturalPersonModel naturalPerson)
        {
            // SQL query to insert a new natural person
            var query = @"
                INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, UserId)
                VALUES (@Id, @FirstName, @FirstSurname, @SecondSurname, @UserId)";

            // Create a SQL command with the query and connection
            using (var command = new SqlCommand(query, _connection))
            {
                // Add parameters to the query to prevent SQL injection
                command.Parameters.AddWithValue("@Id", naturalPerson.Id);
                command.Parameters.AddWithValue("@FirstName", naturalPerson.FirstName);
                command.Parameters.AddWithValue("@FirstSurname", naturalPerson.FirstSurname);
                command.Parameters.AddWithValue("@SecondSurname", naturalPerson.SecondSurname);
                command.Parameters.AddWithValue("@UserId", naturalPerson.UserId != null ? naturalPerson.UserId : DBNull.Value);

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