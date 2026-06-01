using Doantotnghiep.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.ViewModel
{
    public class NhanVienDatLichViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn dịch vụ!")]
        public int IdDatLich { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Nhan Vien!")]
        public int IdNhanVien { get; set; }
        [Required]
        public TrangThaiDatLich TrangThaiDatLich { get; set; }
        [Required]
        public DateTime NgayHenLich { get; set; }
        [Required]
        public DateTime GioBatDauDV { get; set; }
        [Required]
        public DateTime GioKetThucDV { get; set; }
        public string GhiChuDatLich { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
        public string TenDichVu { get; set; } = string.Empty;
    }
}
