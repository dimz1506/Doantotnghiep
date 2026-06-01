using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Services.Implementations
{
    public class HoaDonService : IHoaDonService
    {
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly AppDbContext _context;

        public HoaDonService(IHoaDonRepository hoaDonRepository, AppDbContext context)
        {
            _hoaDonRepository = hoaDonRepository;
            _context = context;
        }

        public async Task<List<HoaDon>> GetAllAsync()
        {
            return await _hoaDonRepository.GetAllAsync();
        }

        public async Task<HoaDon?> GetByDatLichAsync(int idDatLich)
        {
            return await _hoaDonRepository.GetByDatLichAsync(idDatLich);
        }

        public async Task<HoaDon> GetByIdAsync(int id)
        {
            var hoaDon = await _hoaDonRepository.GetByIdAsync(id);

            if (hoaDon == null)
            {
                throw new Exception("Không tìm thấy hóa đơn.");
            }

            return hoaDon;
        }

        public async Task<HoaDon> TaoHoaDonTuDatLichAsync(int idDatLich)
        {
            var hoaDonDaTonTai = await _hoaDonRepository.GetByDatLichAsync(idDatLich);

            if (hoaDonDaTonTai != null)
            {
                throw new Exception("Lịch đặt này đã có hóa đơn.");
            }

            var datLich = await _context.DatLichs
                .Include(x => x.ChiTietDatLichs)
                    .ThenInclude(x => x.DichVu)
                .FirstOrDefaultAsync(x => x.IdDatLich == idDatLich);

            if (datLich == null)
            {
                throw new Exception("Không tìm thấy lịch đặt.");
            }

            if (datLich.ChiTietDatLichs == null || !datLich.ChiTietDatLichs.Any())
            {
                throw new Exception("Lịch đặt chưa có dịch vụ, không thể tạo hóa đơn.");
            }

            var today = DateTime.Today;

            var khuyenMais = await _context.DichVuKhuyenMais
                .Include(x => x.KhuyenMai)
                .Where(x =>
                    x.KhuyenMai != null &&
                    !x.KhuyenMai.IsDeleted &&
                    x.KhuyenMai.TrangThaiKhuyenMai &&
                    x.KhuyenMai.NgayBatDau.Date <= today &&
                    x.KhuyenMai.NgayKetThuc.Date >= today)
                .ToListAsync();

            var chiTietHoaDons = new List<ChiTietHoaDon>();

            foreach (var chiTietDatLich in datLich.ChiTietDatLichs)
            {
                var thanhTien = chiTietDatLich.GiaDichVu * chiTietDatLich.SoLuong;

                var khuyenMaiApDung = khuyenMais
                    .Where(x => x.IdDichVu == chiTietDatLich.IdDichVu)
                    .Select(x => x.KhuyenMai)
                    .Where(x => x != null)
                    .OrderByDescending(x => x!.GiaTriGiam)
                    .FirstOrDefault();

                decimal tienGiam = 0;

                if (khuyenMaiApDung != null)
                {
                    if (khuyenMaiApDung.LoaiKhuyenMai == Models.Enum.LoaiKhuyenMai.GiamTheoPhanTram)
                    {
                        tienGiam = thanhTien * khuyenMaiApDung.GiaTriGiam / 100;
                    }
                    else if (khuyenMaiApDung.LoaiKhuyenMai == Models.Enum.LoaiKhuyenMai.GiamTheoSoTien)
                    {
                        tienGiam = khuyenMaiApDung.GiaTriGiam;
                    }

                    if (tienGiam > thanhTien)
                    {
                        tienGiam = thanhTien;
                    }
                }

                var chiTietHoaDon = new ChiTietHoaDon
                {
                    IdDichVu = chiTietDatLich.IdDichVu,
                    SoLuongDV = chiTietDatLich.SoLuong,
                    DonGiaDV = chiTietDatLich.GiaDichVu,
                    IdKhuyenMai = khuyenMaiApDung?.IdKhuyenMai,
                    TienGiam = tienGiam
                };

                chiTietHoaDons.Add(chiTietHoaDon);
            }

            var tongTienGoc = chiTietHoaDons.Sum(x => x.SoLuongDV * x.DonGiaDV);
            var tongTienGiamGia = chiTietHoaDons.Sum(x => x.TienGiam);
            var tongTienSauGiamGia = tongTienGoc - tongTienGiamGia;

            var hoaDon = new HoaDon
            {
                IdDatLich = idDatLich,
                TongTienGoc = tongTienGoc,
                TongTienGiamGia = tongTienGiamGia,
                TongTienSauGiamGia = tongTienSauGiamGia,
                TrangThaiHoaDon = TrangThaiHoaDon.ChuaThanhToan,
                PhuongThucTT = null,
                NgayThanhToan = null,
                NgayTaoHoaDon = DateTime.Now,
                ChiTietHoaDons = chiTietHoaDons
            };

            await _hoaDonRepository.AddAsync(hoaDon);

            return hoaDon;
        }
         
        public async Task ThanhToanAsync(int idHoaDon, PhuongThucTT phuongThucTT)
        {
            var hoaDon = await _hoaDonRepository.GetByIdAsync(idHoaDon);

            if (hoaDon == null)
            {
                throw new Exception("Không tìm thấy hóa đơn.");
            }

            if (hoaDon.TrangThaiHoaDon == TrangThaiHoaDon.DaThanhToan)
            {
                return;
            }

            if (hoaDon.DatLich != null && hoaDon.DatLich.TrangThaiDatLich == TrangThaiDatLich.DaHuy)
            {
                throw new Exception("Không thể thanh toán hóa đơn của lịch đã hủy.");
            }

            hoaDon.TrangThaiHoaDon = TrangThaiHoaDon.DaThanhToan;
            hoaDon.PhuongThucTT = phuongThucTT;
            hoaDon.NgayThanhToan = DateTime.Now;

            if (hoaDon.DatLich != null)
            {
                hoaDon.DatLich.TrangThaiDatLich = TrangThaiDatLich.DaHoanThanh;
                hoaDon.DatLich.NgayCapNhatDatLich = DateTime.Now;
            }

            await _hoaDonRepository.UpdateAsync(hoaDon);
        }
    }
}
