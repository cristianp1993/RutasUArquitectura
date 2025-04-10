using Microsoft.AspNetCore.Mvc;

namespace ProyectoU2025.Controllers
{
    public class ChatbotController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
