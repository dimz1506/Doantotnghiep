using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.Entities
{
    public class LoaiDichVu
    {
        [Key]
        public int IdLoaiDichVu { get; set; }
        [Required]
        public string TenLoaiDichVu { get; set; } = string.Empty;
        public string MoTaLoaiDichVu { get; set; } = string.Empty;
        [Required]
        public bool TrangThaiLoaiDV { get; set; }
        [Required]
        public DateTime NgayTaoLoaiDichVu { get; set; } = DateTime.Now;
        public ICollection<DichVu>? DichVus { get; set; }
    }
}
