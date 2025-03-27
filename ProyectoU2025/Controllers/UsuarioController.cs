using Microsoft.AspNetCore.Mvc;
using ProyectoU2025.Repositories;

namespace ProyectoU2025.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);

            if (usuario == null)
                return Unauthorized("Usuario no existe");

            return View();
        }
    }
}
