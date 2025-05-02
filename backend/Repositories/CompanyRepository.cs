using backend.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace backend.Services
{
    public class CompanyService
    {
        private readonly SqlConnection _connection;
        private readonly string _connectionString;

        public CompanyService()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("PayrollSystem");
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

        public List<CompanyViewModel> GetCompanies()
        {
            var companies = new List<CompanyViewModel>();
            var query = "SELECT * FROM View_CompanyFullInfo";
            var table = CreateQueryTable(query);

            foreach (DataRow row in table.Rows)
            {
                companies.Add(new CompanyViewModel
                {
                    Id = row["Id"] != DBNull.Value ? Convert.ToString(row["Id"]) : string.Empty,
                    Name = row["Name"] != DBNull.Value ? Convert.ToString(row["Name"]) : string.Empty,
                    PaymentType = row["PaymentType"] != DBNull.Value ? Convert.ToString(row["PaymentType"]) : string.Empty,
                    Province = row["Province"] != DBNull.Value ? Convert.ToString(row["Province"]) : string.Empty,
                    Canton = row["Canton"] != DBNull.Value ? Convert.ToString(row["Canton"]) : string.Empty,
                    District = row["District"] != DBNull.Value ? Convert.ToString(row["District"]) : string.Empty,
                    Neighborhood = row["Neighborhood"] != DBNull.Value ? Convert.ToString(row["Neighborhood"]) : string.Empty,
                    AdditionalDirectionDetails = row["AdditionalDirectionDetails"] != DBNull.Value ? Convert.ToString(row["AdditionalDirectionDetails"]) : string.Empty,
                    LegalId = row["LegalId"] != DBNull.Value ? Convert.ToString(row["LegalId"]) : string.Empty,
                    Email = row["Email"] != DBNull.Value ? Convert.ToString(row["Email"]) : string.Empty,
                    PhoneNumber = row["PhoneNumber"] != DBNull.Value ? Convert.ToString(row["PhoneNumber"]) : string.Empty,
                });
            }

            return companies;
        }

        public bool CreateCompany(Company company)
        {
            try
            {
                const string query = @"
            INSERT INTO Companies
            (Id, Name, Description, PaymentType, MaxBenefitsPerEmployee, CreationDate, CreationAuthor, LastModificationDate, LastModificationAuthor)
            VALUES
            (@Id, @Name, @Description, @PaymentType, @MaxBenefitsPerEmployee, @CreationDate, @CreationAuthor, @LastModificationDate, @LastModificationAuthor)";

                using var command = new SqlCommand(query, _connection);

                command.Parameters.AddWithValue("@Id", company.Id);
                command.Parameters.AddWithValue("@Name", company.Name);
                command.Parameters.AddWithValue("@Description", company.Description);
                command.Parameters.AddWithValue("@PaymentType", company.PaymentType);
                command.Parameters.AddWithValue("@MaxBenefitsPerEmployee", company.MaxBenefitsPerEmployee);
                command.Parameters.AddWithValue("@CreationDate", company.CreationDate);
                command.Parameters.AddWithValue("@CreationAuthor", company.CreationAuthor);
                command.Parameters.AddWithValue("@LastModificationDate", (object?)company.LastModificationDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastModificationAuthor", (object?)company.LastModificationAuthor ?? DBNull.Value);

                _connection.Open();
                var result = command.ExecuteNonQuery() > 0;
                _connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CreateCompany: {ex.Message}");
                return false;
            }
        }
    }
}
