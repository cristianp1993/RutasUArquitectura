using Microsoft.AspNetCore.Mvc;
using ProyectoU2025.Services;

namespace ProyectoU2025.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public async Task<IActionResult> Index()
        {
            var schedule = await _scheduleService.GetScheduleForLoggedUserAsync(User);
            return View(schedule);
        }
    }

}
