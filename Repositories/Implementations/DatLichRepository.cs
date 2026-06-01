using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class DatLichRepository : IDatLichRepository
    {
        private readonly AppDbContext _context;
        public DatLichRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DatLich> CreateDatLichAsync(DatLich datLich)
        {
            _context.DatLichs.Add(datLich);
            await _context.SaveChangesAsync();
            return datLich;

        }

        public async Task<bool> UpdateTrangThaiAsync(int iddatlich, TrangThaiDatLich trangThailich)
        {
            var datlich = await _context.DatLichs.FindAsync(iddatlich);
            if (datlich == null)
            {
                return false;
            }
            datlich.TrangThaiDatLich = trangThailich;
            datlich.NgayCapNhatDatLich = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<List<DatLich>> GetAllDatLichAsync()
        {
            return await _context.DatLichs.AsNoTracking().Include(dl => dl.KhachHang).Include(dl => dl.NhanVien).ToListAsync();
        }

        public async Task<DatLich?> GetDatLichByIdAsync(int id)
        {
            return await _context.DatLichs.AsNoTracking().Include(dl => dl.KhachHang).Include(dl => dl.NhanVien).FirstOrDefaultAsync(dl => dl.IdDatLich == id);
        }

        public async Task<List<DatLich>> GetDatLichByKhachHangIdAsync(int khachHangId)
        {
            return await _context.DatLichs.AsNoTracking().Where(dl => dl.IdKhachHang == khachHangId).Include(dl => dl.KhachHang).Include(dl => dl.NhanVien).ToListAsync();
        }

        public async Task<List<DatLich>> GetDatLichByNgayHenLichAsync(DateTime ngayHenLich)
        {
            var startOfDay = ngayHenLich.Date;
            var endOfDay = ngayHenLich.Date.AddDays(1);
            return await _context.DatLichs.AsNoTracking().Where(dl => dl.NgayHenLich >= startOfDay && dl.NgayHenLich < endOfDay).Include(dl => dl.KhachHang).Include(dl => dl.NhanVien).ToListAsync();
        }

        public async Task<List<DatLich>> GetDatLichByNhanVienIdAsync(int nhanVienId)
        {
            return await _context.DatLichs.AsNoTracking().Where(dl => dl.IdNhanVien == nhanVienId).Include(dl => dl.KhachHang).Include(dl => dl.NhanVien).ToListAsync();
        }

        public async Task<List<DatLich>> GetDatLichByTrangThaiAsync(TrangThaiDatLich trangthai)
        {
            return await _context.DatLichs.AsNoTracking().Where(dl => dl.TrangThaiDatLich == trangthai).Include(dl => dl.KhachHang).Include(dl => dl.NhanVien).ToListAsync();
        }



        public async Task<bool> KiemTraTrungLichAsync(int IdNhanVien, DateTime ngayHenLich, DateTime gioBatDauDV, DateTime gioKetThucDV, int? idDatLich = null)
        {
            return await _context.DatLichs.Where(dl => dl.IdNhanVien == IdNhanVien && dl.NgayHenLich.Date == ngayHenLich.Date && (idDatLich == null || dl.IdDatLich != idDatLich) && dl.TrangThaiDatLich != TrangThaiDatLich.DaHuy)
                .AnyAsync(dl => (gioBatDauDV < dl.GioKetThucDV && gioKetThucDV > dl.GioBatDauDV));
        }

        public async Task<bool> UpdateDatLichAsync(DatLich datLich)
        {
            _context.DatLichs.Update(datLich);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<NhanVien>> GetNhanVienSelectListAsync()
        {
            return await _context.NhanViens.Where(nv => nv.TaiKhoan.IdVaiTro == 2).ToListAsync();
        }
        public async Task<List<DichVu>> GetDichVuSelectListAsync()
        {
            return await _context.DichVus.AsNoTracking().Where(dv => dv.TrangThaiDV == true).ToListAsync();
        }
    }
}
