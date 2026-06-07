using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class TinNhanAI
    {
        [Key]
        public int IdTinNhanAI { get; set; }
        [Required]
        public int IdHoiThoaiAI { get; set; }
        [ForeignKey(nameof(IdHoiThoaiAI))]
        public HoiThoaiAI? HoiThoaiAI { get; set; }
        [Required]
        public string VaiTro { get; set; }= string.Empty;

        [Required]
        public string NoiDung {  get; set; } = string.Empty;

        [Required]
        public DateTime thoiGianGui { get; set; } = DateTime.Now;
    }
}
