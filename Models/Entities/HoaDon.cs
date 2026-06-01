using Doantotnghiep.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class HoaDon
    {
        [Key]
        public int IdHoaDon { get; set; }
        [Required]
        public int IdDatLich { get; set; }
        [ForeignKey("IdDatLich")]
        public DatLich DatLich { get; set; }
        [Required]
        public decimal TongTienGoc { get; set; }
        public decimal TongTienGiamGia { get; set; }
        public decimal TongTienSauGiamGia { get; set; }
        [Required]
        public TrangThaiHoaDon TrangThaiHoaDon { get; set; }
        public PhuongThucTT? PhuongThucTT { get; set; }
        public DateTime? NgayThanhToan { get; set; }
       
        public DateTime NgayTaoHoaDon { get; set; } = DateTime.Now;
        public ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
