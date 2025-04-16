using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoU2025.Models.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace ProyectoU2025.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Mostrar información del usuario si está autenticado
            if (User.Identity.IsAuthenticated)
            {
                ViewData["UserName"] = User.Identity.Name;
                ViewData["UserEmail"] = User.FindFirstValue(ClaimTypes.Email);
                ViewData["UserPicture"] = User.FindFirstValue("picture");
            }
            return View();
        }



        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
