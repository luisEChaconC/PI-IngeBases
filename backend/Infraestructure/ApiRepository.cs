using backend.Domain;
using System.Data.SqlClient;

namespace backend.Repositories
{

    public class APIRepository : IAPIRepository
    {
        private readonly string _connectionString;
        protected SqlConnection _connection;
        
        public APIRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        public List<ApiModel> GetAPIs()
        {
            var apis = new List<ApiModel>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM APIs", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        apis.Add(new ApiModel
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            URL = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Token = reader.IsDBNull(3) ? null : reader.GetString(3),
                            SecurityKeyName = reader.GetString(4),
                            EndpointMethod = reader.GetString(5)
                        });
                    }
                }
            }
            return apis;
        }

        public List<ApiParameterModel> GetParametersByAPI(Guid apiId)
        {
            var parameters = new List<ApiParameterModel>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM APIParameters WHERE APIId = @apiId", conn);
                cmd.Parameters.AddWithValue("@apiId", apiId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        parameters.Add(new ApiParameterModel
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Type = reader.GetString(2),
                            APIId = reader.GetGuid(3)
                        });
                    }
                }
            }
            return parameters;
        }

        public bool AddParameterValue(ParameterValueModel value)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"INSERT INTO ParametersValues 
                    (Id, ParameterId, EmployeeId, ValueType, StringValue, IntValue, DateValue)
                    VALUES (@Id, @ParameterId, @EmployeeId, @ValueType, @StringValue, @IntValue, @DateValue)", conn);

                cmd.Parameters.AddWithValue("@Id", value.Id);
                cmd.Parameters.AddWithValue("@ParameterId", value.ParameterId);
                cmd.Parameters.AddWithValue("@EmployeeId", value.EmployeeId);
                cmd.Parameters.AddWithValue("@ValueType", value.ValueType);
                cmd.Parameters.AddWithValue("@StringValue", (object?)value.StringValue ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IntValue", (object?)value.IntValue ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DateValue", (object?)value.DateValue ?? DBNull.Value);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<ParameterValueModel> GetParameterValues(Guid parameterId)
        {
            var values = new List<ParameterValueModel>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM ParametersValues WHERE ParameterId = @parameterId", conn);
                cmd.Parameters.AddWithValue("@parameterId", parameterId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        values.Add(new ParameterValueModel
                        {
                            Id = reader.GetGuid(0),
                            ParameterId = reader.GetGuid(1),
                            EmployeeId = reader.GetGuid(2),
                            ValueType = reader.GetString(3),
                            StringValue = reader.IsDBNull(4) ? null : reader.GetString(4),
                            IntValue = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            DateValue = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6)
                        });
                    }
                }
            }
            return values;
        }
    }
}