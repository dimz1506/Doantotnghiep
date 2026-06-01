using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class LichLamViecRepository : ILichlamViecRepository
    {
        private readonly AppDbContext _context;
        public LichLamViecRepository (AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LichLamViecNhanVien lichLamViecNV)
        {
            await _context.LichLamViecNhanViens.AddAsync(lichLamViecNV);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LichLamViecNhanVien>> GetAllAsync()
        {
            return await _context.LichLamViecNhanViens
                .Include(x => x.NhanVien)
                .Include(x => x.CaLamViec)
                .OrderByDescending(x => x.NgayLamViecNV)
                .ToListAsync();
        }

        public async Task<LichLamViecNhanVien?> GetByIdAsync(int id)
        {
                return await _context.LichLamViecNhanViens
                .Include(x => x.NhanVien)
                .Include(x => x.CaLamViec)
                .FirstOrDefaultAsync(x => x.IdLichLamViecNhanVien == id);
        }

        

        public async Task<List<LichLamViecNhanVien>> GetByNhanVienAsync(int idNhanVien)
        {
             return await _context.LichLamViecNhanViens
                 .Include(x => x.NhanVien)
                 .Include(x => x.CaLamViec)
                 .Where(x => x.IdNhanVien == idNhanVien)
                 .OrderByDescending(x => x.NgayLamViecNV)
                 .ToListAsync();
        }
    }
}
