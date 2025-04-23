using ProyectoU2025.Models;

namespace ProyectoU2025.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<t_usuario> GetByEmailAsync(string email);
        Task<t_usuario> GetUserByEmailPasswordAsync(string email, string password);
        Task<t_usuario> GetByGoogleIdAsync(string googleId);
        Task<t_usuario> GetByIdAsync(int id);
        Task<int> AddAsync(t_usuario usuario);
        Task UpdateAsync(t_usuario usuario);
        Task<string?> GetStudentCodeByIdAsync(int id);
    }
}
