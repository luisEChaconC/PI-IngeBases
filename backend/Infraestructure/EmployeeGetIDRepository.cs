using System;
using System.Data;
using backend.Domain;
using Microsoft.Data.SqlClient;

namespace backend.Infraestructure
{
    public class EmployeeGetIDRepository
    {
        private SqlConnection _connection;
        private string _connectionString;

        public EmployeeGetIDRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        public EmployeeGetIDModel GetEmployeeById(string id)
        {
            var query = @"
                SELECT 
                    e.Id AS EmployeeId,
                    e.CompanyId,
                    np.FirstName,
                    np.FirstSurname,
                    np.SecondSurname,
                    np.Gender,
                    p.LegalId AS Cedula,
                    e.WorkerId,
                    e.ContractType,
                    e.GrossSalary,
                    e.EmployeeStartDate,
                    e.HasToReportHours,
                    u.Email,
                    u.IsAdmin,
                    c.PhoneNumber,
                    np.Gender  
                FROM 
                    Employees e
                JOIN NaturalPersons np ON e.Id = np.Id
                JOIN Persons p ON np.Id = p.Id
                LEFT JOIN Users u ON np.UserId = u.Id
                LEFT JOIN Contacts c ON p.Id = c.PersonId AND c.Type = 'Phone Number'
                WHERE e.Id = @Id";

            EmployeeGetIDModel employee = null;

            try
            {
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    _connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new EmployeeGetIDModel
                            {
                                Id = reader["EmployeeId"].ToString(),
                                WorkerId = reader["WorkerId"].ToString(),
                                CompanyId = reader["CompanyId"].ToString(),
                                EmployeeStartDate = Convert.ToDateTime(reader["EmployeeStartDate"]),
                                ContractType = reader["ContractType"].ToString(),
                                GrossSalary = Convert.ToDecimal(reader["GrossSalary"]),
                                HasToReportHours = Convert.ToBoolean(reader["HasToReportHours"]),
                                FirstName = reader["FirstName"]?.ToString(),
                                FirstSurname = reader["FirstSurname"]?.ToString(),
                                SecondSurname = reader["SecondSurname"]?.ToString(),
                                Gender = reader["Gender"]?.ToString(),
                                Cedula = reader["Cedula"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                IsAdmin = reader["IsAdmin"] != DBNull.Value ? Convert.ToBoolean(reader["IsAdmin"]) : (bool?)null,
                                PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                Gender = reader["Gender"]?.ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employee: {ex.Message}");
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return employee;
        }
    }
}