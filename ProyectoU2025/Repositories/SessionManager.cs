using Microsoft.AspNetCore.Http;
using ProyectoU2025.Models;
using ProyectoU2025.Repositories.Interfaces;

namespace ProyectoU2025.Repositories
{
    public class SessionManager : ISessionManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetUserSession(t_usuario usuario)
        {
            var session = _httpContextAccessor.HttpContext.Session;

            session.SetString("usu_id", usuario.usu_id.ToString());
            session.SetString("usu_email", usuario.usu_email);
            session.SetString("usu_nombre", usuario.usu_nombre);
            session.SetString("usu_rol", usuario.usu_rol);
        }

        public void ClearUserSession()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            session.Remove("usu_id");
            session.Remove("usu_email");
            session.Remove("usu_nombre");
            session.Remove("usu_rol");
        }
    }
}
