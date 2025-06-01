using System;
using System.Data;
using backend.Domain;
using Microsoft.Data.SqlClient;

namespace backend.Infraestructure
{
    public class EmployerGetIDRepository
    {
        private SqlConnection _connection;
        private string _connectionString;

        public EmployerGetIDRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        public EmployerGetIDModel GetEmployerById(string id)
        {
            var query = @"
                SELECT 
                    em.Id AS EmployerId,
                    em.CompanyId,
                    em.WorkerId,
                    np.FirstName,
                    np.FirstSurname,
                    np.SecondSurname,
                    p.LegalId AS Cedula,
                    u.Email,
                    u.IsAdmin,
                    c.PhoneNumber,
                    np.Gender -- ← agregado aquí
                FROM 
                    Employers em
                JOIN NaturalPersons np ON em.Id = np.Id
                JOIN Persons p ON np.Id = p.Id
                LEFT JOIN Users u ON np.UserId = u.Id
                LEFT JOIN Contacts c ON p.Id = c.PersonId AND c.Type = 'Phone Number'
                WHERE em.Id = @Id";

            EmployerGetIDModel employer = null;

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
                            employer = new EmployerGetIDModel
                            {
                                Id = reader["EmployerId"].ToString(),
                                CompanyId = reader["CompanyId"].ToString(),
                                WorkerId = reader["WorkerId"].ToString(),
                                FirstName = reader["FirstName"]?.ToString(),
                                FirstSurname = reader["FirstSurname"]?.ToString(),
                                SecondSurname = reader["SecondSurname"]?.ToString(),
                                Cedula = reader["Cedula"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                IsAdmin = reader["IsAdmin"] != DBNull.Value ? Convert.ToBoolean(reader["IsAdmin"]) : null,
                                PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                Gender = reader["Gender"]?.ToString() // ← agregado aquí
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employer: {ex.Message}");
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return employer;
        }

    }
}
