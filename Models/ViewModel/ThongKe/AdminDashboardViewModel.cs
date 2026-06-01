using Doantotnghiep.Models.Enum;

namespace Doantotnghiep.Models.ViewModel.ThongKe
{
    public class AdminDashboardViewModel
    {
        public int TongLichDat { get; set; }
        public int LichChoXacNhan { get; set; }
        public int LichDaXacNhan { get; set; }
        public int LichDangThucHien { get; set; }
        public int LichDaHoanThanh { get; set; }
        public int LichHuy { get; set; }

        public int TongHoaDon {  get; set; }
        public int HoaDonChuaThanhToan { get; set; }
        public int HoaDonDaThanhToan { get; set; }

        public decimal DoanhThuDaThanhToan { get; set; }

        public int TongKhachHang {  get; set; }
        public int TongNhanVien {  get; set; }
        public int TongDichVu {  get; set; }

        
    }

    public class DoanhThuTheoNgayViewModel
    {
        public DateTime Ngay {  get; set; }
        public decimal DoanhThu {  get; set; }
        public int SoHoaDon { get; set; }
    }

    public class DoanhThuTheoThangViewModel
    {
        public int Nam {  get; set; }
        public decimal DoanhThu { get; set; }
        public int Thang { get; set; }
        public int SoHoaDon { get; set; }
    }

    public class DichVuBanChayViewModel
    {
        public int IdDichVu { get; set; }
        public string TenDichVu { get; set; }
        public int TongSoLuong { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class LichHenTheoTrangThaiViewModel
    {
        public TrangThaiDatLich TrangThai { get; set; }
        public int SoLuong { get; set; }
    }

    public class NhanVienThongKeViewModel
    {
        public int IdNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public int TongLichPhuTrach { get; set; }
        public int LichHoanThanh { get; set; }
        public decimal DoanhThu { get; set; }
    }
}
