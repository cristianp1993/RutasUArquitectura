using ProyectoU2025.Models;

namespace ProyectoU2025.Repositories
{
    public interface IUsuarioRepository
    {
        Task<t_usuarios> GetByEmailAsync(string email);
        Task<t_usuarios> GetByGoogleIdAsync(string googleId);
        Task<int> AddAsync(t_usuarios usuario);
    }
}
