using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class LoaiTriThucRepository : ILoaiTriThucRepository
    {
        private readonly AppDbContext _context;
        public LoaiTriThucRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LoaiTriThuc loaiTriThuc)
        {
            await _context.LoaiTriThucs.AddAsync(loaiTriThuc);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LoaiTriThuc>> GetAllAsync()
        {
            return await _context.LoaiTriThucs
                .Include(x => x.TriThucs)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.NgayTaoLoaiTriThuc)
                .ToListAsync();
        }

        public async Task<LoaiTriThuc?> GetByIdAsync(int id)
        {
            return await _context.LoaiTriThucs
                .Include(x => x.TriThucs)
                .FirstOrDefaultAsync(x => x.IdLoaiTriThuc == id && !x.IsDeleted);
        }

        public async Task UpdateAsync(LoaiTriThuc loaiTriThuc)
        {
            _context.LoaiTriThucs.Update(loaiTriThuc);
            await _context.SaveChangesAsync();
        }
    }
}
