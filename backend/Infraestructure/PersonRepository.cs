using backend.Domain;
using Microsoft.Data.SqlClient;

namespace backend.Infraestructure
{
    /// <summary>
    /// Repository for managing operations related to the Persons table.
    /// </summary>
    public class PersonRepository
    {
        private SqlConnection _connection; // SQL connection object
        private string _connectionString; // Connection string for the database

        /// <summary>
        /// Constructor to initialize the connection string and SQL connection.
        /// </summary>
        public PersonRepository()
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Inserts a new person into the Persons table.
        /// </summary>
        /// <param name="person">The person model containing the data to insert.</param>
        /// <returns>The database-generated ID of the new person.</returns>
        public string CreatePerson(PersonsModel person)
        {
            string personId = string.Empty;

            // SQL query to insert a new person and return the generated ID
            var query = @"
                INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
                OUTPUT INSERTED.Id
                VALUES (@LegalId, @Type, @Province, @Canton, @Neighborhood, @AdditionalDirectionDetails)";

            // Create a SQL command with the query and connection
            using (var command = new SqlCommand(query, _connection))
            {
                // Add parameters to the query to prevent SQL injection
                command.Parameters.AddWithValue("@LegalId", person.LegalId);
                command.Parameters.AddWithValue("@Type", person.Type);
                command.Parameters.AddWithValue("@Province", (object?)person.Province ?? DBNull.Value);
                command.Parameters.AddWithValue("@Canton", (object?)person.Canton ?? DBNull.Value);
                command.Parameters.AddWithValue("@Neighborhood", (object?)person.Neighborhood ?? DBNull.Value);
                command.Parameters.AddWithValue("@AdditionalDirectionDetails", (object?)person.AdditionalDirectionDetails ?? DBNull.Value);

                // Open the database connection
                _connection.Open();

                // Execute the query and get the inserted ID
                personId = command.ExecuteScalar()?.ToString() ?? string.Empty;

                // Close the database connection
                _connection.Close();
            }

            return personId;
        }

        /// <summary>
        /// Retrieves a person by its ID.
        /// </summary>
        /// <param name="id">The ID of the person to retrieve.</param>
        /// <returns>The person model if found; otherwise, null.</returns>
        public PersonsModel GetPersonById(string id)
        {
            PersonsModel person = null;

            // SQL query to select a person by ID
            var query = @"
                SELECT Id, LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails
                FROM Persons
                WHERE Id = @Id";

            // Create a new connection for this operation
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL command with the query and connection
                    using (var command = new SqlCommand(query, connection))
                    {
                        // Add the ID parameter to the query to prevent SQL injection
                        command.Parameters.AddWithValue("@Id", id);

                        // Execute the query and retrieve the results using a data reader
                        using (var reader = command.ExecuteReader())
                        {
                            // Check if a record was found
                            if (reader.Read())
                            {
                                // Map the data from the reader to a PersonsModel object
                                person = new PersonsModel
                                {
                                    Id = reader["Id"].ToString(),
                                    LegalId = reader["LegalId"].ToString(),
                                    Type = reader["Type"].ToString(),
                                    Province = reader["Province"] != DBNull.Value ? reader["Province"].ToString() : null,
                                    Canton = reader["Canton"] != DBNull.Value ? reader["Canton"].ToString() : null,
                                    Neighborhood = reader["Neighborhood"] != DBNull.Value ? reader["Neighborhood"].ToString() : null,
                                    AdditionalDirectionDetails = reader["AdditionalDirectionDetails"] != DBNull.Value ? reader["AdditionalDirectionDetails"].ToString() : null
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving person: {ex.Message}");
                    throw;
                }
            }

            return person;
        }
    }
}