using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class KhuyenMaiRepository : IKhuyenMaiRepository
    {
        private readonly AppDbContext _context;
        public KhuyenMaiRepository (AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(KhuyenMai khuyenMai)
        {
            await _context.KhuyenMais.AddAsync(khuyenMai);
            await _context.SaveChangesAsync();
        }

        public async Task<List<KhuyenMai>> GetAllAsync()
        {
            return await _context.KhuyenMais.Include( x=> x.DichVuKhuyenMais)
                .ThenInclude( x => x.DichVu)
                .Where(x => !x.IsDeleted)
                .OrderByDescending( x => x.NgayTaoKhuyenMai)
                .ToListAsync();
        }


        public async Task<KhuyenMai?> GetByIdAsync(int id)
        {
            return await _context.KhuyenMais
                .Include( x => x.DichVuKhuyenMais)
                .ThenInclude ( x => x.DichVu)
                .FirstOrDefaultAsync( x => x.IdKhuyenMai == id && !x.IsDeleted);
        }

        public async Task SoftDeleteAsync(KhuyenMai khuyenMai)
        {
            khuyenMai.IsDeleted = true;
            khuyenMai.NgayXoaKhuyenMai = DateTime.Now;
            khuyenMai.TrangThaiKhuyenMai = false;

            _context.KhuyenMais.Update( khuyenMai );
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KhuyenMai khuyenMai)
        {
            _context.KhuyenMais.Update(khuyenMai);
            await _context.SaveChangesAsync();
        }
    }
}
