using ProyectoU2025.Models.ViewModels;

namespace ProyectoU2025.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<ScheduleViewModel>> GetByStudentCodeAsync(string studentCode);
    }
}
