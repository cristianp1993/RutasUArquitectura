using ProyectoU2025.Models.ViewModels;

namespace ProyectoU2025.Repositories.Interfaces
{
    public interface ISalonRepository
    {
       
      Task<SalonViewModel> GetSalonAsync(string tipo, string valorInput);
        
    }
}
