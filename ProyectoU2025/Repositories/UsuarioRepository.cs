using ProyectoU2025.Db;
using ProyectoU2025.Models;
using Dapper;
using ProyectoU2025.Querys;

namespace ProyectoU2025.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDapperContext _context;

        public UsuarioRepository(IDapperContext context)
        {
            _context = context;
        }
        public async Task<t_usuarios> GetByEmailAsync(string email)
        {            

            await using var con = await _context.CreateConnectionAsync();
            return await con.QueryFirstOrDefaultAsync<t_usuarios>(UsuariosQuerys.GetByEmail, new { Email = email });
        }

        public Task<t_usuarios> GetByGoogleIdAsync(string googleId)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(t_usuarios usuario)
        {
            throw new NotImplementedException();
        }
    }
}
