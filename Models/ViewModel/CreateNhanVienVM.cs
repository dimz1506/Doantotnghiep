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
    }
}
