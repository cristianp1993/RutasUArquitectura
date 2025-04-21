using Microsoft.AspNetCore.Mvc;
using ProyectoU2025.Models.ViewModels;
using ProyectoU2025.Services;

namespace ProyectoU2025.Controllers
{
    public class SearchController : Controller
    {
        private readonly SalonService _salonService;

        public IActionResult Index()
        {
            return View();
        }
        public SearchController(SalonService salonService)
        {
            _salonService = salonService;
        }

        [HttpPost("Ubicacion/Salon")]
        public async Task<IActionResult> GetUbicacion([FromBody] SearchRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.tipo) || string.IsNullOrWhiteSpace(request.valorInput))
            {
                return BadRequest("Los parámetros 'tipo' y 'valorInput' son obligatorios.");
            }

            var respuesta = await _salonService.ObtenerUbicacionSalonAsync(request.tipo, request.valorInput);
            return Ok(respuesta);
        }
    }
}
