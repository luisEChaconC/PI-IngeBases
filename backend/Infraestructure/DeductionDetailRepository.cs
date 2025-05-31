using backend.Domain;
using backend.Application; 
using System.Data.SqlClient;

namespace backend.Infraestructure
{
    public class DeductionDetailRepository : IDeductionDetailRepository
    {
        private readonly string _connectionString;

         public DeductionDetailRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void InsertDeductionDetail(DeductionDetailModel detail)
        {
            const string query = @"
                INSERT INTO DeductionDetails (Id, Name, AmountDeduced, PaymentDetailsId, DeductionType)
                VALUES (@Id, @Name, @AmountDeduced, @PaymentDetailsId, @DeductionType)";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", detail.Id);
            command.Parameters.AddWithValue("@Name", detail.Name);
            command.Parameters.AddWithValue("@AmountDeduced", detail.AmountDeduced);
            command.Parameters.AddWithValue("@PaymentDetailsId", detail.PaymentDetailsId);
            command.Parameters.AddWithValue("@DeductionType", detail.DeductionType);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
