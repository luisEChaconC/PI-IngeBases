using backend.Domain;
using System.Data;
using Microsoft.Data.SqlClient;
using backend.Models;

namespace backend.Infraestructure
{
    /// <summary>
    /// Repository for managing operations related to the Companies table.
    /// </summary>
    public class CompanyRepository : ICompanyRepository
    {
        private SqlConnection _connection; // SQL connection object
        private string _connectionString; // Connection string for the database
        private readonly PersonRepository _personRepository; // Instance of PersonRepository for managing persons
        private readonly ContactRepository _contactRepository; // Instance of PersonRepository for managing persons
        private readonly EmployeeRepository _employeeRepository; // Instance of EmployeeRepository for managing employees

        public CompanyRepository()
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Constructor to initialize the connection string and SQL connection.
        /// </summary>
        public CompanyRepository(PersonRepository personRepository, ContactRepository contactRepository, EmployeeRepository employeeRepository)
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);

            _personRepository = personRepository; // Initialize the PersonRepository instance
            _contactRepository = contactRepository; // Initialize the ContactRepository instance
            _employeeRepository = employeeRepository; // Initialize the EmployeeRepository instance
        }


        /// <summary>
        /// Inserts a new company into the Companies table using a pre-assigned ID from the Persons table.
        /// </summary>
        /// <param name="company">The company model containing the data to insert.</param>
        /// <returns>The ID of the newly inserted company.</returns>
        public void CreateCompany(CompanyModel company)
        {
            // SQL query to insert a new company
            var query = @"
                INSERT INTO Companies (Id, Name, Description, PaymentType, MaxBenefitsPerEmployee, CreationDate, CreationAuthor, LastModificationDate, LastModificationAuthor)
                VALUES (@Id, @Name, @Description, @PaymentType, @MaxBenefitsPerEmployee, @CreationDate, @CreationAuthor, @LastModificationDate, @LastModificationAuthor)";

            // Create a new connection for this operation
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL command with the query and connection
                    using (var command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the query to prevent SQL injection
                        command.Parameters.AddWithValue("@Id", company.Id);
                        command.Parameters.AddWithValue("@Name", company.Name);
                        command.Parameters.AddWithValue("@Description", (object?)company.Description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@PaymentType", company.PaymentType);
                        command.Parameters.AddWithValue("@MaxBenefitsPerEmployee", (object?)company.MaxBenefitsPerEmployee ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CreationDate", company.CreationDate);
                        command.Parameters.AddWithValue("@CreationAuthor", (object?)company.CreationAuthor ?? DBNull.Value);
                        command.Parameters.AddWithValue("@LastModificationDate", (object?)company.LastModificationDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@LastModificationAuthor", (object?)company.LastModificationAuthor ?? DBNull.Value);

                        // Execute the query
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    Console.WriteLine($"Error creating company: {ex.Message}");
                    throw; // Re-throw the exception to be handled by the controller
                }
            }
        }

        /// <summary>
        /// Retrieves all companies from the Companies table.
        /// </summary>
        /// <returns>A list of company models.</returns>
        public List<CompanyModel> GetCompanies()
        {
            List<CompanyModel> companies = new List<CompanyModel>();
            var query = @"
        SELECT c.Id, c.Name, c.Description, c.PaymentType, c.MaxBenefitsPerEmployee, 
               c.CreationDate, c.CreationAuthor, c.LastModificationDate, c.LastModificationAuthor, c.isDeleted,
               p.Province, p.Canton, p.Neighborhood, p.AdditionalDirectionDetails, p.LegalId,
               (SELECT COUNT(*) FROM Employees e WHERE e.CompanyId = c.Id) AS EmployeeCount,
               np.FirstName + ' ' + np.FirstSurname + ' ' + ISNULL(np.SecondSurname, '') AS EmployerFullName
        FROM Companies c
        INNER JOIN Persons p ON c.Id = p.Id
        INNER JOIN Employers e ON c.Id = e.CompanyId
        INNER JOIN NaturalPersons np ON e.Id = np.Id
        ORDER BY c.Name";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var company = new CompanyModel
                                {
                                    Id = reader["Id"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                    PaymentType = reader["PaymentType"].ToString(),
                                    MaxBenefitsPerEmployee = reader["MaxBenefitsPerEmployee"] != DBNull.Value ? (int?)reader["MaxBenefitsPerEmployee"] : null,
                                    CreationDate = (DateTime)reader["CreationDate"],
                                    CreationAuthor = reader["EmployerFullName"] != DBNull.Value ? reader["EmployerFullName"].ToString() : null,
                                    LastModificationDate = reader["LastModificationDate"] != DBNull.Value ? (DateTime?)reader["LastModificationDate"] : null,
                                    LastModificationAuthor = reader["LastModificationAuthor"] != DBNull.Value ? reader["LastModificationAuthor"].ToString() : null,
                                    Employees = new List<EmployeeModel>(),
                                    EmployeeCount = Convert.ToInt32(reader["EmployeeCount"]),
                                    LegalId = reader["LegalId"].ToString(),
                                    IsDeleted = reader["isDeleted"] != DBNull.Value && (bool)reader["isDeleted"],
                                };

                                companies.Add(company);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving companies: {ex.Message}");
                    throw;
                }
            }

            return companies;
        }


        /// <summary>
        /// Retrieves a company by its ID.
        /// </summary>
        /// <param name="id">The ID of the company to retrieve.</param>
        /// <returns>The company model if found; otherwise, null.</returns>
        public CompanyModel GetCompanyById(string id)
        {
            CompanyModel company = null;

            // SQL query to select a company by ID
            var query = @"
                SELECT c.Id, c.Name, c.Description, c.PaymentType, c.MaxBenefitsPerEmployee, 
                       c.CreationDate, c.CreationAuthor, c.LastModificationDate, c.LastModificationAuthor, c.isDeleted,
                       p.Province, p.Canton, p.Neighborhood, p.AdditionalDirectionDetails
                FROM Companies c
                INNER JOIN Persons p ON c.Id = p.Id
                WHERE UPPER(c.Id) = UPPER(@Id)";

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
                                // Map the data from the reader to a CompanyModel object
                                company = new CompanyModel
                                {
                                    Id = reader["Id"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                    PaymentType = reader["PaymentType"].ToString(),
                                    MaxBenefitsPerEmployee = reader["MaxBenefitsPerEmployee"] != DBNull.Value ? (int?)reader["MaxBenefitsPerEmployee"] : null,
                                    CreationDate = (DateTime)reader["CreationDate"],
                                    CreationAuthor = reader["CreationAuthor"] != DBNull.Value ? reader["CreationAuthor"].ToString() : null,
                                    LastModificationDate = reader["LastModificationDate"] != DBNull.Value ? (DateTime?)reader["LastModificationDate"] : null,
                                    LastModificationAuthor = reader["LastModificationAuthor"] != DBNull.Value ? reader["LastModificationAuthor"].ToString() : null,
                                    Person = _personRepository.GetPersonById(reader["Id"].ToString()),
                                    Contact = _contactRepository.GetContactsById(reader["Id"].ToString()),
                                    EmployeesDynamic = _employeeRepository.GetEmployeesByCompanyId(reader["Id"].ToString()),
                                    IsDeleted = reader["isDeleted"] != DBNull.Value && Convert.ToBoolean(reader["isDeleted"]),
                                };
                            }
                        }
                    }

                    // Note: Removed employee loading code that was causing the error
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    Console.WriteLine($"Error retrieving company: {ex.Message}");
                    throw; // Re-throw the exception to be handled by the controller
                }
            }

            // Return the company (or null if not found)
            return company;
        }

        public async Task<string> GetPaymentTypeByIdAsync(Guid companyId)
        {
            string paymentType = null;
            var query = @"SELECT PaymentType FROM Companies WHERE Id = @CompanyId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CompanyId", companyId);

                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    paymentType = result.ToString();
                }
            }

            return paymentType;
        }

        private void ValidateDuplicateLegalId(SqlConnection connection, string legalId, string companyId)
        {
            var query = "SELECT COUNT(*) FROM Persons WHERE LegalId = @LegalId AND Id <> @Id";
            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@LegalId", legalId);
                cmd.Parameters.AddWithValue("@Id", companyId);
                if ((int)cmd.ExecuteScalar() > 0)
                    throw new Exception("El ID ya existe.");
            }
        }

        private void ValidateDuplicateCompanyName(SqlConnection connection, string name, string companyId)
        {
            var query = "SELECT COUNT(*) FROM Companies WHERE Name = @Name AND Id <> @Id";
            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Id", companyId);
                if ((int)cmd.ExecuteScalar() > 0)
                    throw new Exception("Nombre de empresa ya existe.");
            }
        }

        private void ValidateDuplicatePhoneNumber(SqlConnection connection, string phoneNumber, string personId)
        {
            var query = "SELECT COUNT(*) FROM Contacts WHERE PhoneNumber = @PhoneNumber AND PersonId <> @Id";
            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@Id", personId);
                if ((int)cmd.ExecuteScalar() > 0)
                    throw new Exception("Teléfono ya existe.");
            }
        }

        private void ValidateDuplicateEmail(SqlConnection connection, string email, string personId)
        {
            using (var cmd = new SqlCommand("ValidateDuplicateEmail", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PersonId", personId);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCompany(UpdateCompanyModel company)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                ValidateDuplicateLegalId(connection, company.Person.LegalId, company.Id);
                ValidateDuplicateCompanyName(connection, company.Name, company.Id);
                ValidateDuplicatePhoneNumber(connection, company.Contact.PhoneNumber, company.Id);
                ValidateDuplicateEmail(connection, company.Contact.Email, company.Id);

                var query = @"
                    UPDATE Companies
                    SET Name = @Name,
                        PaymentType = @PaymentType,
                        LastModificationDate = GETDATE(),
                        MaxBenefitsPerEmployee = @MaxBenefitsPerEmployee
                    WHERE Id = @Id
                    
                    UPDATE Persons
                    SET LegalId = @LegalId,
                        Province = @Province,
                        Canton = @Canton,   
                        Neighborhood = @Neighborhood,
                        AdditionalDirectionDetails = @AdditionalDirectionDetails
                    WHERE Id = @Id;
                    
                    UPDATE Contacts
                    SET
                        PhoneNumber = CASE 
                            WHEN Email IS NULL OR Email = '' THEN @PhoneNumber
                            ELSE PhoneNumber
                        END,
                        Email = CASE 
                            WHEN PhoneNumber IS NULL OR PhoneNumber = '' THEN @Email
                            ELSE Email
                        END
                    WHERE PersonId = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", company.Id);
                    command.Parameters.AddWithValue("@Name", company.Name);
                    command.Parameters.AddWithValue("@LegalId", company.Person.LegalId);
                    command.Parameters.AddWithValue("@PaymentType", company.PaymentType);
                    command.Parameters.AddWithValue("@Province", company.Person.Province);
                    command.Parameters.AddWithValue("@Canton", company.Person.Canton);
                    command.Parameters.AddWithValue("@Neighborhood", company.Person.Neighborhood);
                    command.Parameters.AddWithValue("@AdditionalDirectionDetails", company.Person.AdditionalDirectionDetails);
                    command.Parameters.AddWithValue("@PhoneNumber", company.Contact.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", company.Contact.Email);
                    command.Parameters.AddWithValue("@MaxBenefitsPerEmployee", company.MaxBenefits);

                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void DeleteCompany(string companyId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_DeleteCompany", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyId", Guid.Parse(companyId));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}