using Doantotnghiep.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class LichLamViecNhanVien
    {
        [Key]
        public int IdLichLamViecNhanVien { get; set; }
        [Required]
        public int IdNhanVien { get; set; }
        [ForeignKey(nameof(IdNhanVien))]
        public NhanVien NhanVien { get; set; }
        [Required]
        public int IdCaLamViec { get; set; }

        [ForeignKey(nameof(IdCaLamViec))]
        public virtual CaLamViec? CaLamViec { get; set; }
        public DateTime NgayLamViecNV { get; set; }
        [Required]
        public DateTime GioBatDauCaLamViecNV { get; set; }
        [Required]
        public DateTime GioKetThucCaLamViecNV { get; set; }
        [Required]
        public TrangThaiLichLamViec TrangThaiLichLamViecNV { get; set; } = TrangThaiLichLamViec.DangLam;
    }
}
