using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.ViewModel
{
    public class EditKhachHangViewModel
    {
        [Required]
        public int IdKhachHang { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string TenKhachHang { get; set; } = string.Empty;
        public string DiaChiKhachHang { get; set; } = string.Empty;
        public string GhiChuKH { get; set; } = string.Empty;
        public DateTime NgayTaoKH { get; set; } = DateTime.Now;
    }
}
