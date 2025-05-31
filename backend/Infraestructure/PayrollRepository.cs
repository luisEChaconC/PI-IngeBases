using backend.Domain;
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
                            Id = reader["Id"].ToString(),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            CompanyId = reader["CompanyId"].ToString(),
                            PayrollManagerId = reader["PayrollManagerId"].ToString()
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
    }
}