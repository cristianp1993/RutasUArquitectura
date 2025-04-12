using ProyectoU2025.Models;

namespace ProyectoU2025.Repositories
{
    public interface IUsuarioRepository
    {
        Task<t_usuario> GetByEmailAsync(string email);
        Task<t_usuario> GetByGoogleIdAsync(string googleId);
        Task<int> AddAsync(t_usuario usuario);
        Task UpdateAsync(t_usuario usuario);
    }
}
