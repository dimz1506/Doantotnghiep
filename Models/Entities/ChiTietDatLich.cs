using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class ChiTietDatLich
    {
        [Key]
        public int IdChiTietDatLich { get; set; }
        [Required]
        public int IdDatLich { get; set; }
        [ForeignKey(nameof(IdDatLich))]
        public DatLich DatLich { get; set; }
        [Required]
        public int IdDichVu { get; set; }
        [ForeignKey(nameof(IdDichVu))]
        public DichVu DichVu { get; set; }
        [Required]
        public decimal GiaDichVu { get; set; }
        [Required]
        public int SoLuong { get; set; } = 1;

    }
}
