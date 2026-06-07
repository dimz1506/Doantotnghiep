using Doantotnghiep.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class NhanVien
    {
        [Key]
        public int IdNhanVien { get; set; }
        [Required]
        public int IdTaiKhoan { get; set; }
        [ForeignKey("IdTaiKhoan")]
        public TaiKhoan? TaiKhoan { get; set; }
        [Required]
        public string TenNhanVien { get; set; } = string.Empty;
        public string DiaChiNV{ get; set; } = string.Empty;
        [Required]
        public string ChuyenMonNV { get; set; } = string.Empty;
        [Required]
        public DateTime NgayTaoNV { get; set; } = DateTime.Now;
        [Required]
        public TrangThaiNhanVien TrangThaiNV { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? NgayXoaNV { get; set; }
        public ICollection<NhanVienDichVu>? NhanVienDichVus { get; set; }
        public ICollection<DatLich>? DatLiches { get; set; }
        public ICollection<HoiThoaiAI>? HoiThoaiAIs { get; set; }
        public bool LaNhanVienFullTime { get; set; } = true;

        public TimeSpan GioBatDauLamViec { get; set; } = new TimeSpan(8, 0, 0);

        public TimeSpan GioKetThucLamViec { get; set; } = new TimeSpan(17, 0, 0);
    }
}
