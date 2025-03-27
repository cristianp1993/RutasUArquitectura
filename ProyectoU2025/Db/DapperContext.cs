using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;


namespace ProyectoU2025.Db
{
    public class DapperContext : IDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // Método para crear una conexión sin abrir
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        // Método asíncrono para crear y abrir una conexión
        public async Task<SqlConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
        
    }
}
