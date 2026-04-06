using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.ViewModel
{
    public class RegisterCustomerVM
    {
        [Required]
        public string TenDangNhap { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string MatKhau { get; set; } = string.Empty;
        [Required]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string XacNhanMatKhau { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string? DiaChiKhachHang { get; set; }

    }
}
