using backend.Domain;
using Microsoft.Data.SqlClient;

namespace backend.Infraestructure
{
    /// <summary>
    /// Repository for managing operations related to the Contacts table.
    /// </summary>
    public class ContactRepository
    {
        private SqlConnection _connection; // SQL connection object
        private string _connectionString; // Connection string for the database

        /// <summary>
        /// Constructor to initialize the connection string and SQL connection.
        /// </summary>
        public ContactRepository()
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Inserts a new contact into the Contacts table and retrieves the ID of the created row.
        /// </summary>
        /// <param name="contact">The contact model containing the data to insert.</param>
        /// <returns>The database-generated ID of the new contact.</returns>
        public string CreateContact(ContactModel contact)
        {
            string contactId = string.Empty;

            // SQL query to insert a new contact and return the generated ID
            var query = @"
                INSERT INTO Contacts (Type, PhoneNumber, Email, PersonId)
                OUTPUT INSERTED.Id
                VALUES (@Type, @PhoneNumber, @Email, @PersonId)";

            // Create a SQL command with the query and connection
            using (var command = new SqlCommand(query, _connection))
            {
                // Add parameters to the query to prevent SQL injection
                command.Parameters.AddWithValue("@Type", contact.Type);
                command.Parameters.AddWithValue("@PhoneNumber", (object?)contact.PhoneNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", (object?)contact.Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@PersonId", contact.PersonId);

                // Open the database connection
                _connection.Open();

                // Execute the query and get the inserted ID
                contactId = command.ExecuteScalar()?.ToString() ?? string.Empty;

                // Close the database connection
                _connection.Close();
            }

            return contactId;
        }

        /// <summary>
        /// Retrieves a list of contacts by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact to retrieve.</param>
        /// <returns>The contact model if found; otherwise, null.</returns>
        public List<ContactModel> GetContactsById(string companyId)
        {
            var contacts = new List<ContactModel>();

            var query = @"
                SELECT Id, Type, PhoneNumber, Email, PersonId
                FROM Contacts
                WHERE PersonId = @CompanyId";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contact = new ContactModel
                            {
                                Id = reader["Id"].ToString(),
                                Type = reader["Type"].ToString(),
                                PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : null,
                                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                                PersonId = reader["PersonId"].ToString()
                            };

                            contacts.Add(contact);
                        }
                    }
                }
            }

            return contacts;
        }
    }
}