using Microsoft.AspNetCore.Mvc;

public class ProfileController : Controller
{
    public IActionResult Index()
    {
        return View(); // Esto buscará la vista en /Views/Profile/Index.cshtml
    }
}
