using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.Entities
{
    public class CaLamViec
    {
        [Key]
        public int IdCaLamViec { get; set; }

        [Required]
        public string TenCa { get; set; } = string.Empty;

        [Required]
        public DateTime NgayLam { get; set; }

        [Required]
        public DateTime GioBatDau { get; set; }

        [Required]
        public DateTime GioKetThuc { get; set; }

        public int SoLuongNhanVienToiDa { get; set; }

        public bool MoDangKy { get; set; } = true;

        public virtual ICollection<DangKyCaLamViec>? DangKyCaLamViecs { get; set; }
        public virtual ICollection<LichLamViecNhanVien>? LichLamViecNhanViens { get; set; }
    }
}
