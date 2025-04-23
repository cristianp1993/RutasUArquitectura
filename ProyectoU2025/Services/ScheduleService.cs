using ProyectoU2025.Models.ViewModels;
using ProyectoU2025.Repositories.Interfaces;
using System.Security.Claims;

namespace ProyectoU2025.Services
{
    public class ScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IUsuarioRepository _userRepository;

        public ScheduleService(IScheduleRepository scheduleRepository, IUsuarioRepository userRepository)
        {
            _scheduleRepository = scheduleRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ScheduleViewModel>> GetScheduleForLoggedUserAsync(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Enumerable.Empty<ScheduleViewModel>();

            var studentCode = await _userRepository.GetStudentCodeByIdAsync(userId);

            if (string.IsNullOrWhiteSpace(studentCode))
                return Enumerable.Empty<ScheduleViewModel>();

            return await _scheduleRepository.GetByStudentCodeAsync(studentCode);
        }

    }

}
