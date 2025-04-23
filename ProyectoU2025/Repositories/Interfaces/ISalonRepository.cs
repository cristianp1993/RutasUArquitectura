using ProyectoU2025.Models.ViewModels;

namespace ProyectoU2025.Repositories.Interfaces
{
    public interface ISalonRepository
    {

        Task<List<SalonViewModel>> GetSalonesAsync(string tipo, string valorInput);

    }
}
