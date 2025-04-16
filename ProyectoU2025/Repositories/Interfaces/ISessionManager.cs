using ProyectoU2025.Models;

namespace ProyectoU2025.Repositories.Interfaces
{
    public interface ISessionManager
    {
        void SetUserSession(t_usuario usuario);
        void ClearUserSession();
    }
}
