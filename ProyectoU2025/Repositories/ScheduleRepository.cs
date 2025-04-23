using Dapper;
using ProyectoU2025.Db;
using ProyectoU2025.Models.ViewModels;
using ProyectoU2025.Querys;
using ProyectoU2025.Repositories.Interfaces;

namespace ProyectoU2025.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IDapperContext _context;

        public ScheduleRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScheduleViewModel>> GetByStudentCodeAsync(string studentCode)
        {
            await using var con = await _context.CreateConnectionAsync();

            var result = await con.QueryAsync<ScheduleViewModel>(
                ScheduleQuerys.sp_GetScheduleByStudentCode,
                new { StudentCode = studentCode },
                commandType: System.Data.CommandType.StoredProcedure
            );

            return result;
        }
    }
}
