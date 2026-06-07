using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly AppDbContext _context;
        public HoaDonRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<HoaDon> AddAsync(HoaDon hoaDon)
        {
            await _context.HoaDons.AddAsync(hoaDon);
            var hoadon = await _context.SaveChangesAsync();
            return hoaDon;
        }

        public async Task<List<HoaDon>> GetAllAsync()
        {
            return await _context.HoaDons
                .Include(x => x.DatLich)
                    .ThenInclude(x => x.KhachHang)
                .Include(x => x.DatLich)
                    .ThenInclude(x => x.NhanVien)
                .Include(x => x.ChiTietHoaDons)
                    .ThenInclude(x => x.DichVu)
                .OrderByDescending(x => x.NgayTaoHoaDon)
                .ToListAsync();
        }

        public async Task<HoaDon?> GetByDatLichAsync(int idDatLich)
        {
            return await _context.HoaDons
                .Include(x => x.ChiTietHoaDons)
                .FirstOrDefaultAsync(x => x.IdDatLich == idDatLich);
        }

        public async Task<HoaDon?> GetByIdAsync(int id)
        {
            return await _context.HoaDons
        .Include(x => x.DatLich)
            .ThenInclude(x => x.KhachHang)
        .Include(x => x.DatLich)
            .ThenInclude(x => x.NhanVien)
        .Include(x => x.ChiTietHoaDons)
            .ThenInclude(x => x.DichVu)
        .Include(x => x.ChiTietHoaDons)
            .ThenInclude(x => x.KhuyenMai)
        .FirstOrDefaultAsync(x => x.IdHoaDon == id);
        }

        public async Task UpdateAsync(HoaDon hoaDon)
        {
            _context.HoaDons.Update(hoaDon);
            await _context.SaveChangesAsync();
        }

    }
}
