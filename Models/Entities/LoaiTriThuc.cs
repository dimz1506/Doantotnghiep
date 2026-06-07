using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.Entities
{
    public class LoaiTriThuc
    {
        [Key]  
        public int IdLoaiTriThuc { get; set; }
        [Required]
        public string TenLoaiTriThuc { get; set; } = string.Empty;
        public string? MoTaLoaiTriThuc { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public DateTime NgayTaoLoaiTriThuc { get; set; } = DateTime.Now;

        public DateTime? NgayCapNhatLoaiTriThuc { get; set; }

        public DateTime? NgayXoaLoaiTriThuc { get; set; }

        public ICollection<TriThuc> TriThucs { get; set; } = new List<TriThuc>();
    }
}
