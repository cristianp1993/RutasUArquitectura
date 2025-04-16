using ProyectoU2025.Db;
using ProyectoU2025.Models;
using Dapper;
using System.Data;
using System.Threading.Tasks;
using ProyectoU2025.Querys;
using ProyectoU2025.Repositories.Interfaces;

namespace ProyectoU2025.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDapperContext _context;

        public UsuarioRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<t_usuario> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            await using var con = await _context.CreateConnectionAsync();
            return await con.QueryFirstOrDefaultAsync<t_usuario>(
                UsuariosQuerys.GetByEmail,
                new { usu_email = email }); 
        }

        public async Task<t_usuario> GetUserByEmailPasswordAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            await using var con = await _context.CreateConnectionAsync();
            return await con.QueryFirstOrDefaultAsync<t_usuario>(
                UsuariosQuerys.GetByEmailAndPassword,
                new { usu_email = email, usu_contrasenia = password });
        }

        public async Task<t_usuario> GetByGoogleIdAsync(string googleId)
        {
            if (string.IsNullOrWhiteSpace(googleId)) return null;

            await using var con = await _context.CreateConnectionAsync();
            return await con.QueryFirstOrDefaultAsync<t_usuario>(
                UsuariosQuerys.GetByGoogleId,
                new { usu_google_id = googleId });
        }

        public async Task<int> AddAsync(t_usuario usuario)
        {
            await using var con = await _context.CreateConnectionAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@usu_google_id", usuario.usu_google_id, DbType.String);
            parameters.Add("@usu_email", usuario.usu_email, DbType.String);
            parameters.Add("@usu_nombre", usuario.usu_nombre, DbType.String);
            parameters.Add("@usu_contrasenia", "123456", DbType.String);
            parameters.Add("@usu_rol", usuario.usu_rol ?? "usuario", DbType.String);            
            parameters.Add("@usu_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await con.ExecuteAsync(
                UsuariosQuerys.Insert,
                parameters,
                commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@usu_id");
        }

        public async Task UpdateAsync(t_usuario usuario)
        {
            await using var con = await _context.CreateConnectionAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@usu_id", usuario.usu_id, DbType.Int32);
            parameters.Add("@usu_google_id", usuario.usu_google_id, DbType.String);
            parameters.Add("@usu_email", usuario.usu_email, DbType.String);
            parameters.Add("@usu_rol", usuario.usu_rol, DbType.String);

            await con.ExecuteAsync(
                UsuariosQuerys.Update,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}