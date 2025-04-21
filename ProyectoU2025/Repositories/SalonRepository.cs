using Dapper;
using ProyectoU2025.Db;
using ProyectoU2025.Models.ViewModels;
using ProyectoU2025.Querys;
using ProyectoU2025.Repositories.Interfaces;

namespace ProyectoU2025.Repositories
{
    public class SalonRepository : ISalonRepository
    {
        private readonly IDapperContext _context;
        public SalonRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<SalonViewModel> GetSalonAsync(string tipo, string valorInput)
        {
            // Validar que los parámetros no sean nulos o vacíos
            if (string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(valorInput))
            {
                throw new ArgumentException("Los parámetros 'tipo' y 'valorInput' son obligatorios.");
            }

            await using var con = await _context.CreateConnectionAsync();

            // Ejecutar el procedimiento almacenado con los parámetros
            return await con.QueryFirstOrDefaultAsync<SalonViewModel>(
                SalonQuerys.sp_GetInfoSalon,
                new { SearchType = tipo, SearchParam = valorInput },
                commandType: System.Data.CommandType.StoredProcedure);

        }
    }
}
