using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ProyectoU2025.Repositories;
using ProyectoU2025.Repositories.Interfaces;
using System.Security.Claims;

namespace ProyectoU2025.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISessionManager _sessionManager;
        public UsuarioController(IUsuarioRepository usuarioRepository, ISessionManager sessionManager)
        {
            _usuarioRepository = usuarioRepository;
            _sessionManager = sessionManager;
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);

            if (usuario == null)
                return Unauthorized("Usuario no existe");

            return View();
        }

        // Sobrecarga del método Login (busca por correo y contraseña)
        [HttpPost("/Usuario/Login")]       
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Validar que los datos no estén vacíos
                if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new { success = false, message = "Datos inválidos" });
                }

                // Buscar al usuario en la base de datos
                var usuario = await _usuarioRepository.GetUserByEmailPasswordAsync(request.Email, request.Password);

                if (usuario == null)
                {
                    return Unauthorized(new { success = false, message = "Credenciales inválidas" });
                }

                // Almacenar información del usuario en la sesión
                _sessionManager.SetUserSession(usuario);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.usu_id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.usu_email),
                    new Claim(ClaimTypes.Role, usuario.usu_rol),
                    new Claim(ClaimTypes.Name, usuario.usu_nombre)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(200)
                    });

                return Ok(new { success = true, redirectUrl = "/Profile/Index" });
            }
            catch (Exception ex)
            {                   
                return StatusCode(500, new { success = false, message = "Ocurrió un error interno. Por favor, intenta nuevamente." });
            }
        }


    }
}
