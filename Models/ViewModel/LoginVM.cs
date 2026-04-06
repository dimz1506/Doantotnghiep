using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập!")]
        public string TenDangNhap { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public string MatKhau { get; set; } = string.Empty;
    }
}
