using backend.Domain;
using Microsoft.Data.SqlClient;

namespace backend.Infraestructure
{
    /// <summary>
    /// Repository for managing operations related to the Employees table.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private SqlConnection _connection; // SQL connection object
        private string _connectionString; // Connection string for the database

        /// <summary>
        /// Constructor to initialize the connection string and SQL connection.
        /// </summary>
        public EmployeeRepository()
        {
            // Create a configuration builder to retrieve the connection string
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Initialize the SQL connection with the connection string
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Inserts a new employee into the Employees table.
        /// </summary>
        /// <param name="employee">The employee model containing the data to insert.</param>
        public void CreateEmployee(EmployeeModel employee)
        {
            // SQL query to insert a new employee
            var query = @"
                INSERT INTO Employees (Id, WorkerId, CompanyId, EmployeeStartDate, ContractType, GrossSalary, HasToReportHours)
                VALUES (@Id, @WorkerId, @CompanyId, @EmployeeStartDate, @ContractType, @GrossSalary, @HasToReportHours)";

            // Create a SQL command with the query and connection
            using (var command = new SqlCommand(query, _connection))
            {
                // Add parameters to the query to prevent SQL injection
                command.Parameters.AddWithValue("@Id", employee.Id);
                command.Parameters.AddWithValue("@WorkerId", employee.WorkerId);
                command.Parameters.AddWithValue("@CompanyId", employee.CompanyId);
                command.Parameters.AddWithValue("@EmployeeStartDate", employee.EmployeeStartDate);
                command.Parameters.AddWithValue("@ContractType", employee.ContractType);
                command.Parameters.AddWithValue("@GrossSalary", employee.GrossSalary);
                command.Parameters.AddWithValue("@HasToReportHours", employee.HasToReportHours);

                // Open the database connection
                _connection.Open();

                // Execute the query
                command.ExecuteNonQuery();

                // Close the database connection
                _connection.Close();
            }
        }

        public List<dynamic> GetEmployeesByCompanyId(string companyId)
        {
            var employees = new List<dynamic>();

            var query = @"
                SELECT
                    e.Id as IdEmployee,
                    np.FirstName + ' ' + np.FirstSurname + ' ' + np.SecondSurname AS FullName,
                    p.LegalId,
                    CASE 
                        WHEN s.Id IS NOT NULL THEN 'Supervisor'
                        WHEN pm.Id IS NOT NULL THEN 'Payroll Manager'
                        ELSE 'Collaborator'
                    END AS Position
                FROM Employees e
                INNER JOIN NaturalPersons np ON e.Id = np.Id
                INNER JOIN Persons p ON np.Id = p.Id
                LEFT JOIN Supervisors s ON e.Id = s.Id
                LEFT JOIN PayrollManagers pm ON e.Id = pm.Id
                WHERE e.CompanyId = @CompanyId";

            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@CompanyId", companyId);

                _connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new
                        {
                            IdEmployee = reader["IdEmployee"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            LegalId = reader["LegalId"].ToString(),
                            Position = reader["Position"].ToString()
                        });
                    }
                }

                _connection.Close();
            }

            return employees;
        }

        public async Task<List<EmployeeModel>> GetSummaryByCompanyIdAsync(Guid companyId)
        {
            var employees = new List<EmployeeModel>();
            var query = @"
                SELECT 
                    Id,
                    WorkerId,
                    CompanyId,
                    EmployeeStartDate,
                    ContractType,
                    GrossSalary,
                    HasToReportHours
                FROM Employees
                WHERE CompanyId = @CompanyId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CompanyId", companyId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        employees.Add(new EmployeeModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            WorkerId = reader["WorkerId"].ToString(),
                            CompanyId = reader.GetGuid(reader.GetOrdinal("CompanyId")),
                            EmployeeStartDate = reader.GetDateTime(reader.GetOrdinal("EmployeeStartDate")),
                            ContractType = reader["ContractType"].ToString(),
                            GrossSalary = reader.GetDecimal(reader.GetOrdinal("GrossSalary")),
                            HasToReportHours = reader.GetBoolean(reader.GetOrdinal("HasToReportHours"))
                        });
                    }
                }
            }

            return employees;
        }
    }
}