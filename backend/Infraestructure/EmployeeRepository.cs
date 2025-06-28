using backend.Domain;
using System.Data;
using backend.Application.DTOs;
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
                command.ExecuteNonQuery();
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
                        WHEN pm.Id IS NOT NULL THEN 'PayrollManager'
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

        /// <summary>
        /// Updates an existing employee across multiple tables.
        /// </summary>
        public void UpdateEmployee(UpdateEmployeeModel updated)
        {
            
            var query = @"
                UPDATE Employees
                SET
                    WorkerId = ISNULL(NULLIF(@WorkerId, ''), WorkerId),
                    ContractType = ISNULL(NULLIF(@ContractType, ''), ContractType),
                    GrossSalary = ISNULL(@GrossSalary, GrossSalary)
                WHERE Id = @Id;

                UPDATE NaturalPersons
                SET
                    FirstName = ISNULL(NULLIF(@FirstName, ''), FirstName),
                    FirstSurname = ISNULL(NULLIF(@FirstSurname, ''), FirstSurname),
                    SecondSurname = ISNULL(NULLIF(@SecondSurname, ''), SecondSurname),
                    Gender = ISNULL(NULLIF(@Gender, ''), Gender)
                WHERE Id = @Id;

                UPDATE Persons
                SET
                    LegalId = ISNULL(NULLIF(@LegalId, ''), LegalId)
                WHERE Id = @Id;
               
                UPDATE Contacts
                SET
                    Email = ISNULL(NULLIF(@Email, ''), Email)
                WHERE PersonId = @Id AND Type = 'Email';

                UPDATE Contacts
                SET
                     PhoneNumber = ISNULL(NULLIF(@PhoneNumber, ''), PhoneNumber)
                WHERE PersonId = @Id AND Type = 'Phone Number';
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var checkDuplicates = @"
            SELECT COUNT(*) FROM Persons WHERE LegalId = @LegalId AND Id != @Id;
            SELECT COUNT(*) FROM Employees WHERE WorkerId = @WorkerId AND Id != @Id;
        ";

       var checkLegalIdQuery = "SELECT COUNT(*) FROM Persons WHERE LegalId = @LegalId AND Id != @Id";
using (var checkCmd = new SqlCommand(checkLegalIdQuery, connection))
{
    checkCmd.Parameters.AddWithValue("@LegalId", updated.LegalId ?? "");
    checkCmd.Parameters.AddWithValue("@Id", updated.Id);

    int count = (int)checkCmd.ExecuteScalar();
    if (count > 0)
        throw new InvalidOperationException("CEDULA_DUPLICADA");
}

// Verifie duplicate worker id
var checkWorkerIdQuery = "SELECT COUNT(*) FROM Employees WHERE WorkerId = @WorkerId AND Id != @Id";
using (var checkCmd = new SqlCommand(checkWorkerIdQuery, connection))
{
    checkCmd.Parameters.AddWithValue("@WorkerId", updated.WorkerId ?? "");
    checkCmd.Parameters.AddWithValue("@Id", updated.Id);

    int count = (int)checkCmd.ExecuteScalar();
    if (count > 0)
        throw new InvalidOperationException("WORKERID_DUPLICADO");
}

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", updated.Id);
                        command.Parameters.AddWithValue("@WorkerId", (object?)updated.WorkerId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ContractType", (object?)updated.ContractType ?? DBNull.Value);
                        command.Parameters.AddWithValue("@GrossSalary", (object?)updated.GrossSalary ?? DBNull.Value);

                        command.Parameters.AddWithValue("@FirstName", (object?)updated.FirstName ?? DBNull.Value);
                        command.Parameters.AddWithValue("@FirstSurname", (object?)updated.FirstSurname ?? DBNull.Value);
                        command.Parameters.AddWithValue("@SecondSurname", (object?)updated.SecondSurname ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Gender", (object?)updated.Gender ?? DBNull.Value);


                        command.Parameters.AddWithValue("@LegalId", (object?)updated.LegalId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", (object?)updated.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@PhoneNumber", (object?)updated.PhoneNumber ?? DBNull.Value);

                        command.ExecuteNonQuery();
                        var roles = new[] { "Supervisors", "PayrollManagers" };

                foreach (var table in roles)
                {
                    if (!string.Equals(updated.Role + "s", table, StringComparison.OrdinalIgnoreCase))
                    {
                        var deleteRole = $"DELETE FROM {table} WHERE Id = @Id";
                        using var deleteCmd = new SqlCommand(deleteRole, connection);
                        deleteCmd.Parameters.AddWithValue("@Id", updated.Id);
                        deleteCmd.ExecuteNonQuery();
                    }
                }

                if (updated.Role == "Supervisor" || updated.Role == "PayrollManager")
                {
                    var roleTable = updated.Role + "s"; // Supervisors o PayrollManagers

                    var existsQuery = $"SELECT COUNT(*) FROM {roleTable} WHERE Id = @Id";
                    using var existsCmd = new SqlCommand(existsQuery, connection);
                    existsCmd.Parameters.AddWithValue("@Id", updated.Id);
                    var exists = (int)existsCmd.ExecuteScalar() > 0;

                    if (!exists)
                    {
                        var insertRole = $"INSERT INTO {roleTable} (Id) VALUES (@Id)";
                        using var insertCmd = new SqlCommand(insertRole, connection);
                        insertCmd.Parameters.AddWithValue("@Id", updated.Id);
                        insertCmd.ExecuteNonQuery();
                    }
                }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error updating employee: {ex.Message}");
                                    throw;
                                }
                            }
                        }
        
public bool HasPaymentRecords(string employeeId)
{
    var query = "SELECT dbo.HasPaymentRecords(@EmployeeId) AS Result";


    using (var connection = new SqlConnection(_connectionString))
    using (var command = new SqlCommand(query, connection))
    {
        command.Parameters.AddWithValue("@EmployeeId", employeeId);
        connection.Open();
        var result = command.ExecuteScalar();
        return result != null && Convert.ToBoolean(result);
    }
}

        public async Task<List<EmployeeSummaryDto>> GetSummaryByCompanyIdAsync(Guid companyId)
        {
            var employees = new List<EmployeeSummaryDto>();
            var query = @"
                SELECT 
                    e.Id,
                    e.WorkerId,
                    e.CompanyId,
                    e.EmployeeStartDate,
                    e.ContractType,
                    e.GrossSalary,
                    e.HasToReportHours,
                    np.FirstName,
                    np.FirstSurname,
                    np.SecondSurname,
                    np.UserId,
                    np.Gender
                FROM Employees e
                INNER JOIN NaturalPersons np ON e.Id = np.Id
                WHERE e.CompanyId = @CompanyId
                AND e.IsDeleted = 0";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CompanyId", companyId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        employees.Add(new EmployeeSummaryDto
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            WorkerId = reader["WorkerId"].ToString(),
                            CompanyId = reader.GetGuid(reader.GetOrdinal("CompanyId")),
                            EmployeeStartDate = reader.GetDateTime(reader.GetOrdinal("EmployeeStartDate")),
                            ContractType = reader["ContractType"].ToString(),
                            GrossSalary = reader.GetDecimal(reader.GetOrdinal("GrossSalary")),
                            HasToReportHours = reader.GetBoolean(reader.GetOrdinal("HasToReportHours")),
                            FirstName = reader["FirstName"].ToString(),
                            FirstSurname = reader["FirstSurname"].ToString(),
                            SecondSurname = reader["SecondSurname"].ToString(),
                            UserId = reader["UserId"] == DBNull.Value ? null : reader["UserId"].ToString(),
                            Gender = reader["Gender"].ToString()
                        });
                    }
                }
            }

            return employees;
        }

        public void DeleteEmployee(string employeeId)
        {
    using (var connection = new SqlConnection(_connectionString))
            {
        connection.Open();
        using (var command = new SqlCommand("sp_DeleteEmployee", connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EmployeeId", Guid.Parse(employeeId));
            command.ExecuteNonQuery();
        }
         }
        }

    }
}
