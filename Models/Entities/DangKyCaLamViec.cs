using Doantotnghiep.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class DangKyCaLamViec
    {
        [Key]
        public int IdDangKyca { get; set; }
        [Required]
        public int IdNhanVien { get; set; }
        [ForeignKey(nameof(IdNhanVien))]
        public virtual NhanVien? NhanVien { get; set; }
        [Required]
        public int IdCaLamViec { get; set; }

        [ForeignKey(nameof(IdCaLamViec))]
        public virtual CaLamViec? CaLamViec { get; set; }
        [Required]
        public TrangThaiDangKyCaLam TrangThai { get; set; } = TrangThaiDangKyCaLam.ChoDuyet;
        

    } 
}
