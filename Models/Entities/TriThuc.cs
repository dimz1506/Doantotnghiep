using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class TriThuc
    {
        [Key]
        public int IdTriThuc { get; set; }
        [Required]
        public string TenTriThuc { get; set; } = string.Empty;
        [Required]
        public string NoiDungTriThuc { get; set; } = string.Empty;
        [Required]
        public DateTime NgayNhapTriThuc { get; set; } = DateTime.Now;
        [Required]
        public DateTime? NgayCapNhatTriThuc { get; set; }
        [Required]
        public int IdLoaiTriThuc { get; set; }
        [ForeignKey("IdLoaiTriThuc")]
        public LoaiTriThuc LoaiTriThuc { get; set; }
    }
}
