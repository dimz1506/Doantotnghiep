using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.Entities
{
    public class VaiTro
    {
        [Key]
        public int IdVaiTro { get; set; }
        [Required]
        public string TenVaiTro { get; set; } = string.Empty;
        public ICollection<TaiKhoan> TaiKhoans { get; set; }
    }
}
