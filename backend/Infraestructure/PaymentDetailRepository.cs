using backend.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace backend.Infraestructure
{
    public class PaymentDetailRepository : IPaymentDetailRepository
    {
        private readonly string _connectionString;

        public PaymentDetailRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<Guid> CreateAsync(PaymentDetailModel model)
        {
            var id = Guid.NewGuid();

            using var connection = new SqlConnection(_connectionString);
                var command = new SqlCommand(@"
            INSERT INTO PaymentDetails (Id, PayrollId, EmployeeId, GrossSalary, IssueDate, EmployerCharges)
            VALUES (@Id, @PayrollId, @EmployeeId, @GrossSalary, @IssueDate, @EmployerCharges)", connection);

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@PayrollId", (object?)model.PayrollId ?? DBNull.Value);
            command.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
            command.Parameters.AddWithValue("@GrossSalary", model.GrossSalary);
            command.Parameters.AddWithValue("@IssueDate", model.IssueDate);
            command.Parameters.AddWithValue("@EmployerCharges", (object?)model.EmployerCharges ?? DBNull.Value);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return id;
        }

        public async Task<PaymentDetailModel?> GetByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM PaymentDetails WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return Map(reader);
            }

            return null;
        }

        public async Task<List<PaymentDetailModel>> GetByEmployeeIdAsync(Guid employeeId)
        {
            var results = new List<PaymentDetailModel>();
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM PaymentDetails WHERE EmployeeId = @EmployeeId", connection);
            command.Parameters.AddWithValue("@EmployeeId", employeeId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(Map(reader));
            }

            return results;
        }

        public async Task<List<PaymentDetailModel>> GetByCompanyIdAsync(Guid companyId)
        {
            var results = new List<PaymentDetailModel>();

            var query = @"
                SELECT pd.*
                FROM PaymentDetails pd
                JOIN Employees e ON pd.EmployeeId = e.Id
                WHERE e.CompanyId = @CompanyId";

            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CompanyId", companyId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(Map(reader));
            }

            return results;
        }

        private PaymentDetailModel Map(SqlDataReader reader)
        {
            return new PaymentDetailModel
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                PayrollId = reader["PayrollId"] != DBNull.Value ? reader.GetGuid(reader.GetOrdinal("PayrollId")) : null,
                EmployeeId = reader.GetGuid(reader.GetOrdinal("EmployeeId")),
                GrossSalary = reader.GetDecimal(reader.GetOrdinal("GrossSalary")),
                IssueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                 EmployerCharges = reader["EmployerCharges"] != DBNull.Value
                ? reader.GetDecimal(reader.GetOrdinal("EmployerCharges"))
                : (decimal?)null
            };
        }

         public async Task UpdateEmployerChargesAsync(Guid paymentDetailsId, decimal employerCharges)
            {
                using var connection = new SqlConnection(_connectionString);
                var command = new SqlCommand(@"
                    UPDATE PaymentDetails
                    SET EmployerCharges = @EmployerCharges
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@EmployerCharges", employerCharges);
                command.Parameters.AddWithValue("@Id", paymentDetailsId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

    }
}
