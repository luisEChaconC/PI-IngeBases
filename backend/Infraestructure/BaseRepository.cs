using Microsoft.Data.SqlClient;

namespace backend.Infraestructure
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;
        protected SqlConnection _connection;

        protected BaseRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
    }
}