using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class NhanVienDichVu
    {
        [Key]
        public int IdNhanVienDichVu { get; set; }
        [Required]
        public int IdNhanVien { get; set; }
        [Required]
        [ForeignKey(nameof(IdNhanVien))]
        public NhanVien? NhanVien { get; set; }
        [Required]
        public int IdDichVu { get; set; }
        public DichVu? DichVu { get; set; }
        [Required]
        public bool TrangThaiNVDV { get; set; }
    }
}
