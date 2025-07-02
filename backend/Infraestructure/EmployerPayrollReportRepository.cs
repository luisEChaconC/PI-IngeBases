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

                        // Read employee fields
                        var employeeName = reader.GetString(reader.GetOrdinal("EmployeeName"));
                        var legalId = reader.GetString(reader.GetOrdinal("LegalId"));
                        var employeeType = reader.GetString(reader.GetOrdinal("EmployeeType"));
                        var paymentPeriod = reader.GetString(reader.GetOrdinal("PaymentPeriod"));
                        var paymentDate = reader.GetString(reader.GetOrdinal("PaymentDate"));
                        var grossSalary = reader.GetDecimal(reader.GetOrdinal("GrossSalary"));
                        var employerSocialCharges = Convert.ToDecimal(reader["EmployerSocialCharges"]);
                        var voluntaryDeductions = Convert.ToDecimal(reader["VoluntaryDeductions"]);
                        var employerCost = Convert.ToDecimal(reader["EmployerCost"]);

                        // Only add if there is real employee data
                        if (!string.IsNullOrWhiteSpace(employeeName) 
                            || !string.IsNullOrWhiteSpace(legalId) 
                            || grossSalary > 0 
                            || employerSocialCharges > 0 
                            || voluntaryDeductions > 0 
                            || employerCost > 0)
                        {
                            report.Employees.Add(new EmployeePayrollInfoDto
                            {
                                EmployeeName = employeeName,
                                LegalId = legalId,
                                EmployeeType = employeeType,
                                PaymentPeriod = paymentPeriod,
                                PaymentDate = paymentDate,
                                GrossSalary = grossSalary,
                                EmployerSocialCharges = employerSocialCharges,
                                VoluntaryDeductions = voluntaryDeductions,
                                EmployerCost = employerCost
                            });
                        }
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