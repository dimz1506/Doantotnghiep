using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class HoiThoaiAI
    {
        [Key]
        public int IdHoiThoaiAI { get; set; }
        [Required]
        public int IdKhachHang { get; set; }
        [Required]
        [ForeignKey(nameof(IdKhachHang))]
        public KhachHang KhachHang { get; set; }
        [Required]
        public string NoiDungHoiThoai { get; set; } = string.Empty;
        [Required]
        public string NoiDungTraLoi { get; set; } = string.Empty;
        [Required]
        public DateTime ThoiGianHoiThoai { get; set; } = DateTime.Now;
        [Required]
        public bool TrangThaiChuyenNV { get; set; }
        [Required]
        public int? IdNhanVien { get; set; }
        public NhanVien? NhanVien { get; set; }
    }
}
