using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class ChiTietDatLichRepository : IChiTietDatLichRepository
    {
        private readonly AppDbContext _context;
        public ChiTietDatLichRepository (AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChiTietDatLich chiTietDatLich)
        {
            await _context.ChiTietDatLiches.AddAsync(chiTietDatLich);
        }

        public Task DeleteAsync(ChiTietDatLich chiTietDatLich)
        {
            _context.ChiTietDatLiches.Remove(chiTietDatLich);
            return Task.CompletedTask;
        }

        public async Task<List<ChiTietDatLich>> GetAllAsync()
        {
            return await _context.ChiTietDatLiches
                .Include(x => x.DichVu)
                .Include(x => x.DatLich)
                .ToListAsync();
        }

        public async Task<List<ChiTietDatLich>> GetByDatLichIdAsync(int idDatLich)
        {
            return await _context.ChiTietDatLiches
                .Include(x => x.DichVu)
                .Where(x => x.IdDatLich == idDatLich)
                .ToListAsync();
        }

        public async Task<ChiTietDatLich?> GetByIdAsync(int id)
        {
            return await _context.ChiTietDatLiches
                .Include(x => x.DichVu)
                .Include(x => x.DatLich)
                .FirstOrDefaultAsync(x => x.IdChiTietDatLich == id);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(ChiTietDatLich chiTietDatLich)
        {
            _context.ChiTietDatLiches.Update(chiTietDatLich);
            return Task.CompletedTask;
        }
    }
}
