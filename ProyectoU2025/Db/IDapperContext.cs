using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace ProyectoU2025.Db
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
        Task<SqlConnection> CreateConnectionAsync();
    }
}
