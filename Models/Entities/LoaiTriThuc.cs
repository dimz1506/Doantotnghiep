using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.Entities
{
    public class LoaiTriThuc
    {
        [Key]  
        public int IdLoaiTriThuc { get; set; }
        [Required]
        public string TenLoaiTriThuc { get; set; } = string.Empty;
        [Required]
        public string MoTaLoaiTriThuc { get; set; } = string.Empty;
        
        public ICollection<TriThuc> TriThucs { get; set; }
    }
}
