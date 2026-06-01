using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.ViewModel
{
    public class KhachHangDatLichViewModel
    {
        public int IdDatLich { get; set; }
        public DateTime NgayHenLich { get; set; }
        public string GhiChuDatLich { get; set; } = string.Empty;
        public int IdNhanVien { get; set; }
        //public string TenNhanVien { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vui lòng chọn dịch vụ!")]
        public string TenDichVu { get; set; } = string.Empty;
        public DateTime GioBatDauDV { get; set; }
        //public DateTime GioKetThucDV { get; set; }
    }
}
