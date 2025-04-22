using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoU2025.Models;
using ProyectoU2025.Repositories;
using ProyectoU2025.Repositories.Interfaces;
using System.Security.Claims;

namespace ProyectoU2025.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISessionManager _sessionManager;


        public AccountController(IUsuarioRepository usuarioRepository, ISessionManager sessionManager)
        {
            _usuarioRepository = usuarioRepository;
            _sessionManager = sessionManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/Profile/Index")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin(string returnUrl = "/Profile/Index")
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse", new { returnUrl = returnUrl })
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/Profile/Index")
        {
            // Autenticar al usuario con Google
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Error al autenticar con Google";
                return RedirectToAction("Error");
            }

            // Obtener información del usuario de Google
            var googleId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name);

            // Validar que el correo pertenezca al dominio @ucaldas.edu.co
            if (!email.EndsWith("@ucaldas.edu.co"))
            {
                TempData["Error"] = "Solo se permiten correos del dominio @ucaldas.edu.co";
                return RedirectToAction("Error");
            }

            // Buscar usuario en la base de datos
            var usuario = await _usuarioRepository.GetByGoogleIdAsync(googleId) ??
                          await _usuarioRepository.GetByEmailAsync(email);

            if (usuario == null)
            {
                // Crear nuevo usuario
                usuario = new t_usuario
                {
                    usu_google_id = googleId,
                    usu_nombre = name,
                    usu_email = email,
                    usu_rol = "usuario", // Rol por defecto                  
                    usu_contrasenia = "123456"
                };

                usuario.usu_id = await _usuarioRepository.AddAsync(usuario);
            }
            else if (string.IsNullOrEmpty(usuario.usu_google_id))
            {
                // Actualizar usuario existente con Google ID si no lo tenía
                usuario.usu_google_id = googleId;
                await _usuarioRepository.UpdateAsync(usuario);
            }

            // Almacenar información del usuario en la sesión 
            _sessionManager.SetUserSession(usuario);

            // Crear una identidad de usuario y firmar una cookie de autenticación
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.usu_id.ToString()),
                new Claim(ClaimTypes.Email, usuario.usu_email),
                new Claim(ClaimTypes.Role, usuario.usu_rol)
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

            return LocalRedirect(returnUrl);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}