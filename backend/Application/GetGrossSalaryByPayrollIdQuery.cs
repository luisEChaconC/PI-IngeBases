using Microsoft.Data.SqlClient;

public class GetGrossSalaryByPayrollIdQuery : IGetGrossSalaryByPayrollIdQuery
{
    private readonly string _connectionString;

    public GetGrossSalaryByPayrollIdQuery(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public decimal Execute(Guid payrollId)
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
