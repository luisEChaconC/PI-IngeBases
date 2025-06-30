using backend.Application.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace backend.Infraestructure
{
    public class EmployerPayrollReportRepository : IEmployerPayrollReportRepository
    {
        private readonly string _connectionString;

        public EmployerPayrollReportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<EmployerEmployeePayrollReportDto> GetEmployeePayrollReport(Guid companyId, Guid employerId)
        {
            var report = new EmployerEmployeePayrollReportDto
            {
                Employees = new List<EmployeePayrollInfoDto>()
            };

            using var connection = new SqlConnection(_connectionString);
            try
            {
                var command = new SqlCommand("sp_GetEmployeePayrollReport", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@CompanyId", companyId);
                command.Parameters.AddWithValue("@EmployerId", employerId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        report.CompanyName ??= reader.GetString(reader.GetOrdinal("CompanyName"));
                        report.EmployerName ??= reader.GetString(reader.GetOrdinal("EmployerName"));

                        report.Employees.Add(new EmployeePayrollInfoDto
                        {
                            EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                            LegalId = reader.GetString(reader.GetOrdinal("LegalId")),
                            EmployeeType = reader.GetString(reader.GetOrdinal("EmployeeType")),
                            PaymentPeriod = reader.GetString(reader.GetOrdinal("PaymentPeriod")),
                            PaymentDate = reader.GetString(reader.GetOrdinal("PaymentDate")),
                            GrossSalary = reader.GetDecimal(reader.GetOrdinal("GrossSalary")),
                            EmployerSocialCharges = Convert.ToDecimal(reader["EmployerSocialCharges"]),
                            VoluntaryDeductions = Convert.ToDecimal(reader["VoluntaryDeductions"]),
                            EmployerCost = Convert.ToDecimal(reader["EmployerCost"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employee payroll report: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
            return report;
        }
    }
}