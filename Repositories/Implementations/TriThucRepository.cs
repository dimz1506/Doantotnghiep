using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class TriThucRepository : ITriThucRepository
    {
        private readonly AppDbContext _context;
        public TriThucRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TriThuc entity)
        {
            await _context.TriThucs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TriThuc>> GetAllAsync()
        {
            return await _context.TriThucs
                .Include(x => x.LoaiTriThuc)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.NgayNhapTriThuc)
                .ToListAsync();
        }

        public async Task<TriThuc?> GetByIdAsync(int id)
        {
            return await _context.TriThucs
                .Include(x => x.LoaiTriThuc)
                .FirstOrDefaultAsync(x => x.IdTriThuc == id && !x.IsDeleted);
        }

        public async Task UpdateAsync(TriThuc entity)
        {
            _context.TriThucs.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
