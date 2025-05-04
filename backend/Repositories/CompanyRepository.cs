using backend.Models;
using Microsoft.Data.SqlClient;

namespace backend.Repositories
{
    /// <summary>
    /// Repository for managing operations related to the Companies table.
    /// </summary>
    public class CompanyRepository
    {
        private SqlConnection _connection; // SQL connection object
        private string _connectionString; // Connection string for the database

        /// <summary>
        /// Constructor to initialize the connection string and SQL connection.
        /// </summary>
        public CompanyRepository()
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);
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

            // Create a SQL command with the query and connection
            using (var command = new SqlCommand(query, _connection))
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

                // Open the database connection
                _connection.Open();

                // Execute the query
                command.ExecuteNonQuery();

                // Close the database connection
                _connection.Close();
            }
        }

        /// <summary>
        /// Retrieves all companies from the Companies table.
        /// </summary>
        /// <returns>A list of company models.</returns>
        public List<CompanyModel> GetCompanies()
        {
            // Create a list to store the companies
            List<CompanyModel> companies = new List<CompanyModel>();

            // SQL query to select all companies
            var query = @"
                SELECT c.Id, c.Name, c.Description, c.PaymentType, c.MaxBenefitsPerEmployee, 
                       c.CreationDate, c.CreationAuthor, c.LastModificationDate, c.LastModificationAuthor,
                       p.Province, p.Canton, p.Neighborhood, p.AdditionalDirectionDetails
                FROM Companies c
                INNER JOIN Persons p ON c.Id = p.Id
                ORDER BY c.Name";

            try
            {
                // Create a SQL command with the query and connection
                using (var command = new SqlCommand(query, _connection))
                {
                    // Open the database connection
                    _connection.Open();

                    // Execute the query and retrieve the results using a data reader
                    using (var reader = command.ExecuteReader())
                    {
                        // Iterate through the results
                        while (reader.Read())
                        {
                            // Create a CompanyModel for each row
                            var company = new CompanyModel
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
                                Employees = new List<EmployeeModel>()
                            };

                            // Add the company to the list
                            companies.Add(company);
                        }
                    }
                }
            }
            finally
            {
                // Make sure to close the connection
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }

            // Return the list of companies
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
                       c.CreationDate, c.CreationAuthor, c.LastModificationDate, c.LastModificationAuthor,
                       p.Province, p.Canton, p.Neighborhood, p.AdditionalDirectionDetails
                FROM Companies c
                INNER JOIN Persons p ON c.Id = p.Id
                WHERE c.Id = @Id";

            try
            {
                // Create a SQL command with the query and connection
                using (var command = new SqlCommand(query, _connection))
                {
                    // Add the ID parameter to the query to prevent SQL injection
                    command.Parameters.AddWithValue("@Id", id);

                    // Open the database connection
                    _connection.Open();

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
                                Employees = new List<EmployeeModel>()
                            };
                        }
                    }

                    // If we found the company, get its employees
                    if (company != null)
                    {
                        // Create a new command to get the employees
                        using (var employeeCommand = new SqlCommand(@"
                            SELECT e.Id, e.WorkerId, e.EmployeeStartDate, e.ContractType, e.GrossSalary, e.HasToReportHours,
                                   np.FirstName, np.FirstSurname, np.SecondSurname
                            FROM Employees e
                            INNER JOIN NaturalPersons np ON e.Id = np.Id
                            WHERE e.CompanyId = @CompanyId", _connection))
                        {
                            employeeCommand.Parameters.AddWithValue("@CompanyId", id);

                            // Execute the query and retrieve the results
                            using (var employeeReader = employeeCommand.ExecuteReader())
                            {
                                // Iterate through the results
                                while (employeeReader.Read())
                                {
                                    // Create an EmployeeModel for each row
                                    var employee = new EmployeeModel
                                    {
                                        Id = employeeReader["Id"].ToString(),
                                        WorkerId = employeeReader["WorkerId"].ToString(),
                                        CompanyId = id,
                                        EmployeeStartDate = (DateTime)employeeReader["EmployeeStartDate"],
                                        ContractType = employeeReader["ContractType"].ToString(),
                                        GrossSalary = (decimal)employeeReader["GrossSalary"],
                                        HasToReportHours = (bool)employeeReader["HasToReportHours"]
                                    };

                                    // Add the employee to the company's employees list
                                    company.Employees.Add(employee);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                // Make sure to close the connection
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }

            // Return the company (or null if not found)
            return company;
        }
    }
}