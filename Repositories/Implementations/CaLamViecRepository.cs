using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class CaLamViecRepository : ICaLamViecRepository
    {
        private readonly AppDbContext _appDbContext;
        public CaLamViecRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(CaLamViec caLamViec)
        {
            await _appDbContext.CaLamViecs.AddAsync(caLamViec);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<CaLamViec>> GetAllAsync()
        {
            return await _appDbContext.CaLamViecs.OrderByDescending(x => x.NgayLam).ToListAsync();
        }

        public async Task<CaLamViec?> GetByIdAsync(int id)
        {
            return await _appDbContext.CaLamViecs.FirstOrDefaultAsync(x => x.IdCaLamViec == id);
        }

        public async Task<List<CaLamViec>> GetCaDangMoAsync()
        {
            return await _appDbContext.CaLamViecs
                .Where(x => x.MoDangKy == true)
                .OrderByDescending(x => x.NgayLam)
                .ToListAsync();
        }

        public async Task UpdateAsync(CaLamViec caLamViec)
        {
            _appDbContext.CaLamViecs.Update(caLamViec);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
