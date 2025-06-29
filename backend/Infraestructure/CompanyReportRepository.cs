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
                    ISNULL(SUM(pd.GrossSalary), 0) AS SalarioBruto,
                    ISNULL(SUM(dd.CargasSociales), 0) AS CargasSociales,
                    ISNULL(SUM(dd.DeduccionesVoluntarias), 0) AS DeduccionesVoluntarias,
                    ISNULL(SUM(pd.GrossSalary), 0) + ISNULL(SUM(dd.CargasSociales), 0) AS CostoEmpleador
                FROM Payrolls p
                INNER JOIN Companies c ON p.CompanyId = c.Id
                LEFT JOIN PaymentDetails pd ON pd.PayrollId = p.Id
                LEFT JOIN (
                    SELECT 
                        PaymentDetailsId,
                        SUM(CASE WHEN DeductionType = 'mandatory' THEN AmountDeduced ELSE 0 END) AS CargasSociales,
                        SUM(CASE WHEN DeductionType = 'voluntary' THEN AmountDeduced ELSE 0 END) AS DeduccionesVoluntarias
                    FROM DeductionDetails
                    GROUP BY PaymentDetailsId
                ) dd ON dd.PaymentDetailsId = pd.Id
                WHERE p.EndDate BETWEEN @StartDate AND @EndDate
                GROUP BY c.Name, c.PaymentType, p.StartDate, p.EndDate, p.Id
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
                    SalarioBruto = reader.IsDBNull(reader.GetOrdinal("SalarioBruto")) ? 0 : reader.GetDecimal(reader.GetOrdinal("SalarioBruto")),
                    CargasSociales = reader.IsDBNull(reader.GetOrdinal("CargasSociales")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CargasSociales")),
                    DeduccionesVoluntarias = reader.IsDBNull(reader.GetOrdinal("DeduccionesVoluntarias")) ? 0 : reader.GetDecimal(reader.GetOrdinal("DeduccionesVoluntarias")),
                    CostoEmpleador = reader.IsDBNull(reader.GetOrdinal("CostoEmpleador")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CostoEmpleador"))
                });
            }
            return reports;
        }

        
    }
}