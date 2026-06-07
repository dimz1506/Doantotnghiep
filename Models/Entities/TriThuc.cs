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
        public string? TuKhoa {  get; set; }
        [Required]
        public DateTime NgayNhapTriThuc { get; set; } = DateTime.Now;
        public DateTime? NgayCapNhatTriThuc { get; set; }
        [Required]
        public bool TrangThaiTriThuc { get; set; } = true;
        [Required]
        public bool IsDeleted {  get; set; } = false;
        public DateTime? NgayXoaTriThuc { get; set; }
        [Required]
        public int IdLoaiTriThuc { get; set; }
        [ForeignKey(nameof(IdLoaiTriThuc))]
        public LoaiTriThuc? LoaiTriThuc { get; set; }
    }
}
