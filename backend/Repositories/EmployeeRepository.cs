using backend.Models;
using Microsoft.Data.SqlClient;

namespace backend.Repositories
{
    /// <summary>
    /// Repository for managing operations related to the Employees table.
    /// </summary>
    public class EmployeeRepository
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
                    np.FirstName + ' ' + np.FirstSurname + ' ' + np.SecondSurname AS FullName,
                    p.LegalId,
                    CASE 
                        WHEN s.Id IS NOT NULL THEN 'Supervisor'
                        WHEN pm.Id IS NOT NULL THEN 'Payroll Manager'
                        ELSE 'Employee'
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
    }
}