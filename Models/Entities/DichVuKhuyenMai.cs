using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class DichVuKhuyenMai
    {
        [Key]
        public int IdDichVuKhuyenMai { get; set; }
        [Required]
        public int IdDichVu { get; set; }
        [ForeignKey(nameof(IdDichVu))]
        public DichVu? DichVu { get; set; }
        [Required]
        public int IdKhuyenMai { get; set; }
        [ForeignKey(nameof(IdKhuyenMai))]
        public KhuyenMai? KhuyenMai { get; set; }
    }
}
