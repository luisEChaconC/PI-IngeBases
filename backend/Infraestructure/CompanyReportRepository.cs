using backend.Application.DTOs;
using backend.Domain;
using System.Data.SqlClient;

namespace backend.Repositories
{

    public class CompanyReportRepository : ICompanyReportRepository
    {
        private readonly string _connectionString;
        protected SqlConnection _connection;

        public CompanyReportRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
        
        public async Task<List<CompanyReportDto>> GetCompanyReportsAsync(DateTime startDate, DateTime endDate)
        {
            var reports = new List<CompanyReportDto>();
            var query = @"
                SELECT 
                    c.Name AS Nombre,
                    c.PaymentType AS FrecuenciaPago,
                    CONCAT(FORMAT(p.StartDate, 'dd/MM/yyyy'), ' al ', FORMAT(p.EndDate, 'dd/MM/yyyy')) AS PeriodoPago,
                    FORMAT(p.EndDate, 'dd/MM/yyyy') AS FechaPago,
                    SUM(pd.GrossSalary) AS SalarioBruto,
                    SUM(CASE WHEN dd.DeductionType = 'mandatory' THEN dd.AmountDeduced ELSE 0 END) AS CargasSociales,
                    SUM(CASE WHEN dd.DeductionType = 'voluntary' THEN dd.AmountDeduced ELSE 0 END) AS Deducciones,
                    SUM(pd.GrossSalary) + SUM(CASE WHEN dd.DeductionType = 'mandatory' THEN dd.AmountDeduced ELSE 0 END) AS CostoEmpleador
                FROM Companies c
                INNER JOIN Employees e ON c.Id = e.CompanyId
                INNER JOIN PaymentDetails pd ON e.Id = pd.EmployeeId
                INNER JOIN Payrolls p ON pd.PayrollId = p.Id
                LEFT JOIN DeductionDetails dd ON dd.PaymentDetailsId = pd.Id
                WHERE p.EndDate BETWEEN @StartDate AND @EndDate
                GROUP BY c.Name, c.PaymentType, p.StartDate, p.EndDate
                ORDER BY p.EndDate DESC
            ";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                reports.Add(new CompanyReportDto
                {
                    Nombre = reader["Nombre"].ToString(),
                    FrecuenciaPago = reader["FrecuenciaPago"].ToString(),
                    PeriodoPago = reader["PeriodoPago"].ToString(),
                    FechaPago = reader["FechaPago"].ToString(),
                    SalarioBruto = reader.GetDecimal(reader.GetOrdinal("SalarioBruto")),
                    CargasSociales = reader.GetDecimal(reader.GetOrdinal("CargasSociales")),
                    Deducciones = reader.GetDecimal(reader.GetOrdinal("Deducciones")),
                    CostoEmpleador = reader.GetDecimal(reader.GetOrdinal("CostoEmpleador"))
                });
            }
            return reports;
        }

        
    }
}