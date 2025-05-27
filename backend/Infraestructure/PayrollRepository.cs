using backend.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace backend.Infraestructure
{
    public class PayrollRepository : BaseRepository, IPayrollRepository
    {

        public PayrollRepository() : base()
        {
        }

        public List<PayrollModel> GetPayrollsByCompanyId(string companyId)
        {
            var payrolls = new List<PayrollModel>();
            var query = @"SELECT Id, StartDate, EndDate, CompanyId, PayrollManagerId FROM Payrolls WHERE CompanyId = @CompanyId";
            try
            {
                _connection.Open();
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", companyId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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