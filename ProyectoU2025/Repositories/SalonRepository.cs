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

        public async Task<List<SalonViewModel>> GetSalonesAsync(string tipo, string valorInput)
        {
            if (string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(valorInput))
            {
                throw new ArgumentException("Los parámetros 'tipo' y 'valorInput' son obligatorios.");
            }

            await using var con = await _context.CreateConnectionAsync();

            var result = await con.QueryAsync<SalonViewModel>(
                SalonQuerys.sp_GetInfoSalon,
                new { SearchType = tipo, SearchParam = valorInput },
                commandType: System.Data.CommandType.StoredProcedure
            );

            return result.ToList();
        }

    }
}
