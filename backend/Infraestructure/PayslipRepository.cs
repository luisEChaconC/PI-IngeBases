using backend.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace backend.Infraestructure
{
    public class PayslipRepository : IPayslipRepository
    {
        private readonly string _connectionString;

        public PayslipRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<PayslipModel>> GetByEmployeeIdAsync(Guid employeeId)
        {
            var payslips = new List<PayslipModel>();

            var query = @"
                SELECT pd.Id,
                       CONCAT(np.FirstName, ' ', np.FirstSurname, ' ', np.SecondSurname) AS EmployeeName,
                       c.Name AS CompanyName,
                       c.PaymentType,
                       CONCAT(FORMAT(p.StartDate, 'dd-MM-yy'), ' / ', FORMAT(p.EndDate, 'dd-MM-yy')) AS DateRange,
                       DATENAME(MONTH, pd.IssueDate) AS PaymentMonth,
                       pd.GrossSalary,
                       (pd.GrossSalary - ISNULL(SUM(dd.AmountDeduced), 0)) AS NetPay,
                       pd.Id AS PaymentDetailsId
                FROM PaymentDetails pd
                INNER JOIN Employees e ON pd.EmployeeId = e.Id
                INNER JOIN NaturalPersons np ON e.Id = np.Id
                INNER JOIN Companies c ON e.CompanyId = c.Id
                INNER JOIN Payrolls p ON pd.PayrollId = p.Id
                LEFT JOIN DeductionDetails dd ON dd.PaymentDetailsId = pd.Id
                WHERE pd.EmployeeId = @EmployeeId
                GROUP BY pd.Id, np.FirstName, np.FirstSurname, np.SecondSurname, c.Name, c.PaymentType, p.StartDate, p.EndDate, pd.IssueDate, pd.GrossSalary
                ORDER BY p.EndDate DESC;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EmployeeId", employeeId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var payslipId = reader["PaymentDetailsId"].ToString();
                var items = await GetDeductionItemsAsync(Guid.Parse(payslipId!));

                payslips.Add(new PayslipModel
                {
                    Id = reader["Id"].ToString(),
                    EmployeeName = reader["EmployeeName"].ToString(),
                    CompanyName = reader["CompanyName"].ToString(),
                    PeriodType = TranslatePeriodType(reader["PaymentType"].ToString()),
                    DateRange = reader["DateRange"].ToString(),
                    PaymentMonth = reader["PaymentMonth"].ToString(),
                    GrossSalary = reader.GetDecimal(reader.GetOrdinal("GrossSalary")),
                    NetPay = reader.GetDecimal(reader.GetOrdinal("NetPay")),
                    Items = items
                });
            }

            return payslips;
        }

        public async Task<PayslipModel?> GetByEmployeeIdAndStartDateAsync(Guid employeeId, DateTime startDate)
        {
            PayslipModel? payslip = null;

            var query = @"
                SELECT TOP 1 pd.Id,
                       CONCAT(np.FirstName, ' ', np.FirstSurname, ' ', np.SecondSurname) AS EmployeeName,
                       c.Name AS CompanyName,
                       c.PaymentType,
                       CONCAT(FORMAT(p.StartDate, 'dd-MM-yy'), ' / ', FORMAT(p.EndDate, 'dd-MM-yy')) AS DateRange,
                       DATENAME(MONTH, pd.IssueDate) AS PaymentMonth,
                       pd.GrossSalary,
                       (pd.GrossSalary - ISNULL(SUM(dd.AmountDeduced), 0)) AS NetPay,
                       pd.Id AS PaymentDetailsId
                FROM PaymentDetails pd
                INNER JOIN Employees e ON pd.EmployeeId = e.Id
                INNER JOIN NaturalPersons np ON e.Id = np.Id
                INNER JOIN Companies c ON e.CompanyId = c.Id
                INNER JOIN Payrolls p ON pd.PayrollId = p.Id
                LEFT JOIN DeductionDetails dd ON dd.PaymentDetailsId = pd.Id
                WHERE pd.EmployeeId = @EmployeeId AND p.StartDate = @StartDate
                GROUP BY pd.Id, np.FirstName, np.FirstSurname, np.SecondSurname, c.Name, c.PaymentType, p.StartDate, p.EndDate, pd.IssueDate, pd.GrossSalary;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EmployeeId", employeeId);
            command.Parameters.AddWithValue("@StartDate", startDate);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var payslipId = reader["PaymentDetailsId"].ToString();
                var items = await GetDeductionItemsAsync(Guid.Parse(payslipId!));

                payslip = new PayslipModel
                {
                    Id = reader["Id"].ToString(),
                    EmployeeName = reader["EmployeeName"].ToString(),
                    CompanyName = reader["CompanyName"].ToString(),
                    PeriodType = TranslatePeriodType(reader["PaymentType"].ToString()),
                    DateRange = reader["DateRange"].ToString(),
                    PaymentMonth = reader["PaymentMonth"].ToString(),
                    GrossSalary = reader.GetDecimal(reader.GetOrdinal("GrossSalary")),
                    NetPay = reader.GetDecimal(reader.GetOrdinal("NetPay")),
                    Items = items
                };
            }

            return payslip;
        }

        private async Task<List<PayslipItem>> GetDeductionItemsAsync(Guid paymentDetailsId)
        {
            var voluntaryItems = new List<PayslipItem>();
            var mandatoryItems = new List<PayslipItem>();
            decimal voluntaryTotal = 0;
            decimal mandatoryTotal = 0;

            var query = @"
        SELECT Name, AmountDeduced, DeductionType
        FROM DeductionDetails
        WHERE PaymentDetailsId = @PaymentDetailsId;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PaymentDetailsId", paymentDetailsId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var deductionType = reader["DeductionType"].ToString()?.ToLower();
                var amount = reader.GetDecimal(reader.GetOrdinal("AmountDeduced"));
                var item = new PayslipItem
                {
                    Label = reader["Name"].ToString(),
                    Amount = -amount, // siempre negativo
                    IsBold = false // todos los ítems individuales son false
                };

                if (deductionType == "voluntary")
                {
                    voluntaryItems.Add(item);
                    voluntaryTotal += amount;
                }
                else if (deductionType == "mandatory")
                {
                    mandatoryItems.Add(item);
                    mandatoryTotal += amount;
                }
            }

            var result = new List<PayslipItem>();

            // Voluntarias primero
            result.AddRange(voluntaryItems);

            // Total voluntarias (siempre lo agregamos, aunque sea 0)
            result.Add(new PayslipItem
            {
                Label = "Total Deducciones Voluntarias",
                Amount = -voluntaryTotal,
                IsBold = true
            });

            // Mandatorias después
            result.AddRange(mandatoryItems);

            // Total mandatorias
            result.Add(new PayslipItem
            {
                Label = "Total Deducciones Obligatorias",
                Amount = -mandatoryTotal,
                IsBold = true
            });

            return result;
        }


        private static string TranslatePeriodType(string periodType)
        {
            return periodType switch
            {
                "Weekly" => "SEMANAL",
                "Biweekly" => "QUINCENAL",
                "Monthly" => "MENSUAL",
                _ => "DESCONOCIDO"
            };
        }
    }
}