using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.Entities
{
    public class KhachHang
    {
        [Key]
        public int IdKhachHang { get; set; }
        [Required]
        public int IdTaiKhoan { get; set; }
        [ForeignKey(nameof(IdTaiKhoan))]
        public TaiKhoan TaiKhoan { get; set; }
        [Required]
        public string TenKhachHang { get; set; } = string.Empty;
        public string DiaChiKhachHang { get; set; } = string.Empty;
        public string GhiChuKH { get; set; } = string.Empty;
        [Required]
        public DateTime NgayTaoKH { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public DateTime? NgayXoaKH { get; set; }
        public ICollection<DatLich> DatLiches { get; set; }
        public ICollection<HoiThoaiAI> HoiThoaiAIs { get; set; }

    }
}
