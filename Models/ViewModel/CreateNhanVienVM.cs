using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doantotnghiep.Models.ViewModel
{
    public class CreateNhanVienVM
    {
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string XacNhanMatKhau { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TenNhanVien { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string? DiaChiNhanVien { get; set; }

        public string ChuyenMonNV { get; set; } = string.Empty;

        public bool LaNhanVienFullTime { get; set; } = true;
        public TimeSpan GioBatDauLamViec { get; set; } = new TimeSpan(8, 0, 0);
        public TimeSpan GioKetThucLamViec { get; set; } = new TimeSpan(17, 0, 0);

        public List<int> IdDichVus { get; set; } = new();
        public IEnumerable<SelectListItem> DichVus { get; set; } = new List<SelectListItem>();
    }
}