
namespace Doantotnghiep.Models.ViewModel
{
    public class LoginResultVM
    {
        public int IdTaiKhoan { get; set; }
        public string TenTaiKhoan { get; set; } = string.Empty;
        public int IdVaiTro { get; set; }
        public int? IdNhanVien { get; set; }
        public int? IdKhachHang { get; set; }
    }
}
