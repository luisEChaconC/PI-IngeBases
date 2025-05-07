using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Services
{
    public class BenefitService
    {
        private readonly SqlConnection _connection;
        private readonly string _connectionString;

        public BenefitService()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        private DataTable CreateQueryTable(string query)
        {
            using var command = new SqlCommand(query, _connection);
            using var adapter = new SqlDataAdapter(command);
            var resultTable = new DataTable();

            _connection.Open();
            adapter.Fill(resultTable);
            _connection.Close();

            return resultTable;
        }

        private List<string> GetEmployeeTypesForBenefit(Guid benefitId)
        {
            var employeeTypes = new List<string>();
            var query = @"
                SELECT ET.Name 
                FROM EmployeeTypesXBenefits ETB
                INNER JOIN EmployeeTypes ET ON ET.Id = ETB.EmployeeTypeId
                WHERE ETB.BenefitId = @BenefitId";

            using var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@BenefitId", benefitId);

            _connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                employeeTypes.Add(reader["Name"].ToString());
            }
            _connection.Close();

            return employeeTypes;
        }

        private Guid? GetEmployeeTypeIdByName(string employeeTypeName, SqlConnection connection)
        {
            const string query = "SELECT Id FROM EmployeeTypes WHERE Name = @Name";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", employeeTypeName);

            var result = command.ExecuteScalar();
            return result != null ? (Guid?)result : null;
        }

        public List<Benefit> GetBenefits(string companyId)
        {
            var benefits = new List<Benefit>();
            var query = "SELECT * FROM Benefits WHERE CompanyId = @CompanyId";

            using var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@CompanyId", Guid.Parse(companyId));
            using var adapter = new SqlDataAdapter(command);
            var table = new DataTable();

            _connection.Open();
            adapter.Fill(table);
            _connection.Close();

            foreach (DataRow row in table.Rows)
            {
                var id = Guid.Parse(row["Id"].ToString());
                benefits.Add(new Benefit
                {
                    Id = id.ToString(),
                    CompanyId = row["CompanyId"].ToString(),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    IsActive = Convert.ToBoolean(row["IsActive"]),
                    Type = row["Type"].ToString(),
                    LinkAPI = row["LinkAPI"].ToString(),
                    FixedPercentage = row["FixedPercentage"] != DBNull.Value ? Convert.ToInt32(row["FixedPercentage"]) : null,
                    FixedAmount = row["FixedAmount"] != DBNull.Value ? Convert.ToInt32(row["FixedAmount"]) : null,
                    RequiredMonthsWorked = Convert.ToInt32(row["RequiredMonthsWorked"]),
                    EligibleEmployeeTypes = GetEmployeeTypesForBenefit(id)
                });
            }

            return benefits;
        }

        public bool CreateBenefit(Benefit benefit)
        {
            var benefitId = Guid.NewGuid();

            const string insertBenefitQuery = @"
                INSERT INTO Benefits
                (Id, CompanyId, Name, Description, IsActive, Type, LinkAPI, FixedPercentage, FixedAmount, RequiredMonthsWorked)
                VALUES
                (@Id, @CompanyId, @Name, @Description, @IsActive, @Type, @LinkAPI, @FixedPercentage, @FixedAmount, @RequiredMonthsWorked)";

            using (var connection = new SqlConnection(_connection.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(insertBenefitQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", benefitId);
                    command.Parameters.AddWithValue("@CompanyId", Guid.Parse(benefit.CompanyId));
                    command.Parameters.AddWithValue("@Name", benefit.Name);
                    command.Parameters.AddWithValue("@Description", benefit.Description ?? "");
                    command.Parameters.AddWithValue("@IsActive", benefit.IsActive);
                    command.Parameters.AddWithValue("@Type", benefit.Type);
                    command.Parameters.AddWithValue("@LinkAPI", (object?)benefit.LinkAPI ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FixedPercentage", (object?)benefit.FixedPercentage ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FixedAmount", (object?)benefit.FixedAmount ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RequiredMonthsWorked", benefit.RequiredMonthsWorked);

                    var success = command.ExecuteNonQuery() > 0;

                    foreach (var empType in benefit.EligibleEmployeeTypes)
                    {
                        var empTypeId = GetEmployeeTypeIdByName(empType, connection);
                        if (empTypeId != null)
                        {
                            const string relQuery = @"
                                INSERT INTO EmployeeTypesXBenefits (EmployeeTypeId, BenefitId)
                                VALUES (@EmpTypeId, @BenefitId)";

                            using var relCmd = new SqlCommand(relQuery, connection);
                            relCmd.Parameters.AddWithValue("@EmpTypeId", empTypeId);
                            relCmd.Parameters.AddWithValue("@BenefitId", benefitId);
                            relCmd.ExecuteNonQuery();
                        }
                    }

                    return success;
                }
            }
        }
    }
}
