using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class ChiTietHoaDon
    {
        [Key]
        public int IdChiTietHoaDon { get; set; }
        [Required]
        public int IdHoaDon { get; set; }
        [ForeignKey(nameof(IdHoaDon))]
        public HoaDon HoaDon { get; set; }
        [Required]
        public int IdDichVu { get; set; }
        [ForeignKey(nameof(IdDichVu))]
        public DichVu DichVu { get; set; }
        [Required]
        public int SoLuongDV { get; set; }
        [Required]
        public decimal DonGiaDV { get; set; }
        public int? IdKhuyenMai { get; set; }
        [ForeignKey(nameof(IdKhuyenMai))]
        public KhuyenMai? KhuyenMai { get; set; }
        [Required]
        public decimal TienGiam { get; set; } = 0;

        [NotMapped]
        public decimal ThanhTien => SoLuongDV * DonGiaDV;

        [NotMapped]
        public decimal ThanhTienSauGiam => ThanhTien - TienGiam;
    }
}
