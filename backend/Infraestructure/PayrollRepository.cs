using backend.Domain;
using backend.Application.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace backend.Infraestructure
{
    public class PayrollRepository : IPayrollRepository
    {

        private readonly string _connectionString;

        public PayrollRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<Guid> CreateAsync(PayrollModel model)
        {
            var id = Guid.NewGuid();

            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(@"
                INSERT INTO Payrolls (Id, StartDate, EndDate, CompanyId, PayrollManagerId)
                VALUES (@Id, @StartDate, @EndDate, @CompanyId, @PayrollManagerId)", connection);

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@StartDate", model.StartDate);
            command.Parameters.AddWithValue("@EndDate", model.EndDate);
            command.Parameters.AddWithValue("@CompanyId", model.CompanyId);
            command.Parameters.AddWithValue("@PayrollManagerId", model.PayrollManagerId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return id;
        }

        public async Task<List<PayrollModel>> GetByCompanyIdAsync(Guid companyId)
        {
            var payrolls = new List<PayrollModel>();

            var query = @"SELECT Id, StartDate, EndDate, CompanyId, PayrollManagerId FROM Payrolls WHERE CompanyId = @CompanyId ORDER BY EndDate DESC";
            using var _connection = new SqlConnection(_connectionString);
            try
            {
                var command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@CompanyId", companyId);

                await _connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        payrolls.Add(new PayrollModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            CompanyId = reader.GetGuid(reader.GetOrdinal("CompanyId")),
                            PayrollManagerId = reader.GetGuid(reader.GetOrdinal("PayrollManagerId"))
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving payrolls: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
            return payrolls;
        }

        public async Task<List<PayrollSummaryDto>> GetSummaryByCompanyIdAsync(Guid companyId)
        {
            var summaries = new List<PayrollSummaryDto>();

            var query = "sp_GetPayrollSummaryByCompanyId";
            using var _connection = new SqlConnection(_connectionString);
            try
            {
                using var command = new SqlCommand(query, _connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CompanyId", companyId);

                await _connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        summaries.Add(new PayrollSummaryDto
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("PayrollId")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            PayrollManagerFullName = reader.GetString(reader.GetOrdinal("PayrollManagerFullName")),
                            TotalGrossSalary = reader.GetDecimal(reader.GetOrdinal("TotalGrossSalary")),
                            TotalAmountDeducted = reader.GetDecimal(reader.GetOrdinal("TotalAmountDeducted"))
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving payroll summary: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
            return summaries;
        }
        
        public async Task<bool> CheckPayrollExistsAsync(Guid companyId, DateTime startDate, DateTime endDate)
        {
            var query = "SELECT dbo.fn_CheckPayrollExists(@CompanyId, @StartDate, @EndDate)";
            using var _connection = new SqlConnection(_connectionString);
            try
            {
                using var command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@CompanyId", companyId);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                await _connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking payroll existence: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }
        public decimal GetTotalGrossSalaryByPayrollId(Guid payrollId)
        {
            var total = 0m;

            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT SUM(GrossSalary)
                FROM PaymentDetails
                WHERE PayrollId = @PayrollId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PayrollId", payrollId);

            connection.Open();
            var result = command.ExecuteScalar();
            if (result != DBNull.Value)
                total = Convert.ToDecimal(result);

            return total;
        }

    }
}