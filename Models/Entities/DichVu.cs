using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class DichVu
    {
        [Key]
        public int IdDichVu { get; set; }
        [Required]
        public int IdLoaiDichVu { get; set; }
        [ForeignKey(nameof(IdLoaiDichVu))]
        public LoaiDichVu? LoaiDichVu { get; set; }
        [Required]
        public string TenDichVu { get; set; } = string.Empty;
        [Required]
        public string MoTaDichVu { get; set; } = string.Empty;
        [Required]
        public decimal GiaDichVu { get; set; }
        [Required]
        public bool TrangThaiDV { get; set; }
        [Required]
        public int ThoiLuongDV { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime NgayTaoDichVu { get; set; } = DateTime.Now;
        public DateTime? NgayXoaDV { get; set; }
        public string HinhAnhDichVu { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? HinhAnhFile { get; set; }
        public ICollection<NhanVienDichVu>? NhanVienDichVus { get; set; }
        public ICollection<DatLich>? DatLiches { get; set; }
        public ICollection<ChiTietHoaDon>? ChiTietHoaDons { get; set; }
        public ICollection<DichVuKhuyenMai>? DichVuKhuyenMais { get; set; }
        public ICollection<ChiTietDatLich>? ChiTietDatLichs { get; set; }
    }
}
