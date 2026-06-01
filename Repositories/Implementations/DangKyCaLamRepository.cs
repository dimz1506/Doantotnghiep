using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class DangKyCaLamRepository : IDangKyCaLamRepository
    {
        private readonly AppDbContext _context;
        public DangKyCaLamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DangKyCaLamViec dangkycalam)
        {
            await _context.DangKyCaLamViecs.AddAsync(dangkycalam);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DangKyCaLamViec>> GetAllAsync()
        {
            return await _context.DangKyCaLamViecs
                .Include(x => x.NhanVien)
                .Include(x => x.CaLamViec)
                .OrderByDescending(x => x.IdDangKyca)
                .ToListAsync();
        }

        public async Task<DangKyCaLamViec?> GetByIdAsync(int id)
        {
            return await _context.DangKyCaLamViecs
       .Include(x => x.NhanVien)
       .Include(x => x.CaLamViec)
       .FirstOrDefaultAsync(x => x.IdDangKyca == id);
        }

        public async Task<List<DangKyCaLamViec>> GetByNhanVienAsync(int idNhanVien)
        {
            return await _context.DangKyCaLamViecs
                .Include(x => x.NhanVien)
                .Include(x => x.CaLamViec)
                .Where(x => x.IdNhanVien == idNhanVien)
                .OrderByDescending(x => x.IdDangKyca)
                .ToListAsync();
        }

        public async Task UpdateAsync(DangKyCaLamViec suaDangKycaLam)
        {
            _context.DangKyCaLamViecs.Update(suaDangKycaLam);
            await _context.SaveChangesAsync();
        }
    }
}
