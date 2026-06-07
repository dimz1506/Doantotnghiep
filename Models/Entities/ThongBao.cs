using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class ThongBao
    {
        [Key]
        public int IdThongBao { get; set; }

        public int? IdKhachHang { get; set; }

        [ForeignKey(nameof(IdKhachHang))]
        public KhachHang? KhachHang { get; set; }

        public int? IdNhanVien { get; set; }

        [ForeignKey(nameof(IdNhanVien))]
        public NhanVien? NhanVien { get; set; }

        [Required]
        public string TieuDe { get; set; } = string.Empty;

        [Required]
        public string NoiDung { get; set; } = string.Empty;

        public string? DuongDan { get; set; }

        public bool DaDoc { get; set; } = false;

        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
