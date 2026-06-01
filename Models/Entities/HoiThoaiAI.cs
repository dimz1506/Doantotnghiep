using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class HoiThoaiAI
    {
        [Key]
        public int IdHoiThoaiAI { get; set; }

        public int? IdKhachHang { get; set; }

        [ForeignKey(nameof(IdKhachHang))]
        public KhachHang? KhachHang { get; set; }

        public int? IdNhanVien { get; set; }

        [ForeignKey(nameof(IdNhanVien))]
        public NhanVien? NhanVien { get; set; }

        [Required]
        public DateTime ThoiGianBatDau { get; set; } = DateTime.Now;

        public DateTime? ThoiGianKetThuc { get; set; }

        [Required]
        public bool CanNhanVienTuVan { get; set; } = false;

        [Required]
        public string TrangThaiHoiThoai { get; set; } = "DangChat";
        // DangChat / ChoNhanVien / DaChuyenNhanVien / DaKetThuc

        public string? GhiChuNoiBo { get; set; }

        public ICollection<TinNhanAI> TinNhanAIs { get; set; } = new List<TinNhanAI>();
    }
}