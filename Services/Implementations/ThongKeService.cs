using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
using Doantotnghiep.Models.ViewModel.ThongKe;
using Doantotnghiep.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Doantotnghiep.Services.Implementations
{
    public class ThongKeService : IThongKeService
    {
        private readonly AppDbContext _context;
        public ThongKeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardViewModel> GetDashBoardAsync()
        {
            var doanhThuDaThanhToan = await _context.HoaDons
                .Where(x => x.TrangThaiHoaDon == TrangThaiHoaDon.DaThanhToan)
                .SumAsync(x => (decimal?)x.TongTienSauGiamGia) ?? 0;

            var model = new AdminDashboardViewModel
            {
                TongLichDat = await _context.DatLichs.CountAsync(),

                LichChoXacNhan = await _context.DatLichs.CountAsync(x => x.TrangThaiDatLich == TrangThaiDatLich.ChoXacNhan),
                LichDaXacNhan = await _context.DatLichs.CountAsync(x => x.TrangThaiDatLich == TrangThaiDatLich.DaXacNhan),
                LichDangThucHien = await _context.DatLichs.CountAsync(x => x.TrangThaiDatLich == TrangThaiDatLich.DangThucHien),
                LichDaHoanThanh = await _context.DatLichs.CountAsync(x => x.TrangThaiDatLich == TrangThaiDatLich.DaHoanThanh),
                LichHuy = await _context.DatLichs.CountAsync(x => x.TrangThaiDatLich == TrangThaiDatLich.DaHuy),

                TongHoaDon = await _context.HoaDons.CountAsync(),
                HoaDonChuaThanhToan = await _context.HoaDons.CountAsync(x => x.TrangThaiHoaDon == TrangThaiHoaDon.ChuaThanhToan),
                HoaDonDaThanhToan = await _context.HoaDons.CountAsync(x => x.TrangThaiHoaDon == TrangThaiHoaDon.DaThanhToan),

                DoanhThuDaThanhToan = doanhThuDaThanhToan,

                TongKhachHang = await _context.KhachHangs.CountAsync(),
                TongNhanVien = await _context.NhanViens.CountAsync(),
                TongDichVu = await _context.DichVus.CountAsync()
            };

            return model;
        }

        public async Task<List<DichVuBanChayViewModel>> GetDichVuBanChayAsync(DateTime? tuNgay, DateTime? denNgay)
        {
            var query = _context.ChiTietHoaDons
                .Include(x => x.DichVu)
                .Include(x => x.HoaDon)
                .Where(x => x.HoaDon.TrangThaiHoaDon == TrangThaiHoaDon.DaThanhToan)
                .AsQueryable();

            if (tuNgay.HasValue)
            {
                var start = tuNgay.Value.Date;
                query = query.Where(x => x.HoaDon.NgayThanhToan != null && x.HoaDon.NgayThanhToan >= start);
            }

            if (denNgay.HasValue)
            {
                var end = denNgay.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.HoaDon.NgayThanhToan != null && x.HoaDon.NgayThanhToan <= end);
            }

            return await query
                .GroupBy(x => new
                {
                    x.IdDichVu,
                    TenDichVu = x.DichVu.TenDichVu
                })
                .Select(g => new DichVuBanChayViewModel
                {
                    IdDichVu = g.Key.IdDichVu,
                    TenDichVu = g.Key.TenDichVu,
                    TongSoLuong = g.Sum(x => x.SoLuongDV),
                    DoanhThu = g.Sum(x => x.SoLuongDV * x.DonGiaDV)
                })
                .OrderByDescending(x => x.TongSoLuong)
                .ToListAsync();
        }

        public async Task<List<DoanhThuTheoNgayViewModel>> GetDoanhThuTheoNgayAsync(DateTime tuNgay, DateTime denNgay)
        {
            tuNgay = tuNgay.Date;
            denNgay = denNgay.Date.AddDays(1).AddTicks(-1);

            return await _context.HoaDons
                .Where(x => x.TrangThaiHoaDon == TrangThaiHoaDon.DaThanhToan
                         && x.NgayThanhToan != null
                         && x.NgayThanhToan >= tuNgay
                         && x.NgayThanhToan <= denNgay)
                .GroupBy(x => x.NgayThanhToan!.Value.Date)
                .Select(g => new DoanhThuTheoNgayViewModel
                {
                    Ngay = g.Key,
                    DoanhThu = g.Sum(x => x.TongTienSauGiamGia),
                    SoHoaDon = g.Count()
                })
                .OrderBy(x => x.Ngay)
                .ToListAsync();
        }

        public async Task<List<DoanhThuTheoThangViewModel>> GetDoanhThuTheoThangAsync(int nam)
        {
            return await _context.HoaDons
                .Where(x => x.TrangThaiHoaDon == TrangThaiHoaDon.DaThanhToan
                         && x.NgayThanhToan != null
                         && x.NgayThanhToan.Value.Year == nam)
                .GroupBy(x => new
                {
                    Nam = x.NgayThanhToan!.Value.Year,
                    Thang = x.NgayThanhToan!.Value.Month
                })
                .Select(g => new DoanhThuTheoThangViewModel
                {
                    Nam = g.Key.Nam,
                    Thang = g.Key.Thang,
                    DoanhThu = g.Sum(x => x.TongTienSauGiamGia),
                    SoHoaDon = g.Count()
                })
                .OrderBy(x => x.Thang)
                .ToListAsync();
        }

        public async Task<List<LichHenTheoTrangThaiViewModel>> GetLichHenTheoTrangThaiAsync()
        {
            return await _context.DatLichs
                .GroupBy(x => x.TrangThaiDatLich)
                .Select(g => new LichHenTheoTrangThaiViewModel
                {
                    TrangThai = g.Key,
                    SoLuong = g.Count()
                })
                .OrderBy(x => x.TrangThai)
                .ToListAsync();
        }

        public async Task<List<NhanVienThongKeViewModel>> GetThongKeNhanVienAsync(DateTime? tuNgay, DateTime? denNgay)
        {
            var query = _context.DatLichs
               .Include(x => x.NhanVien)
               .AsQueryable();

            if (tuNgay.HasValue)
            {
                var start = tuNgay.Value.Date;
                query = query.Where(x => x.NgayHenLich >= start);
            }

            if (denNgay.HasValue)
            {
                var end = denNgay.Value.Date;
                query = query.Where(x => x.NgayHenLich <= end);
            }

            var thongKeLich = await query
                .GroupBy(x => new
                {
                    x.IdNhanVien,
                    TenNhanVien = x.NhanVien.TenNhanVien
                })
                .Select(g => new NhanVienThongKeViewModel
                {
                    IdNhanVien = g.Key.IdNhanVien,
                    TenNhanVien = g.Key.TenNhanVien,
                    TongLichPhuTrach = g.Count(),
                    LichHoanThanh = g.Count(x => x.TrangThaiDatLich == TrangThaiDatLich.DaHoanThanh),
                    DoanhThu = 0
                })
                .ToListAsync();

            var hoaDonQuery = _context.HoaDons
                .Include(x => x.DatLich)
                .Where(x => x.TrangThaiHoaDon == TrangThaiHoaDon.DaThanhToan)
                .AsQueryable();

            if (tuNgay.HasValue)
            {
                var start = tuNgay.Value.Date;
                hoaDonQuery = hoaDonQuery.Where(x => x.NgayThanhToan != null && x.NgayThanhToan >= start);
            }

            if (denNgay.HasValue)
            {
                var end = denNgay.Value.Date.AddDays(1).AddTicks(-1);
                hoaDonQuery = hoaDonQuery.Where(x => x.NgayThanhToan != null && x.NgayThanhToan <= end);
            }

            var doanhThuNhanVien = await hoaDonQuery
                .GroupBy(x => x.DatLich.IdNhanVien)
                .Select(g => new
                {
                    IdNhanVien = g.Key,
                    DoanhThu = g.Sum(x => x.TongTienSauGiamGia)
                })
                .ToListAsync();

            foreach (var item in thongKeLich)
            {
                item.DoanhThu = doanhThuNhanVien
                    .FirstOrDefault(x => x.IdNhanVien == item.IdNhanVien)?.DoanhThu ?? 0;
            }

            return thongKeLich
                .OrderByDescending(x => x.DoanhThu)
                .ThenByDescending(x => x.TongLichPhuTrach)
                .ToList();
        }
    }
}