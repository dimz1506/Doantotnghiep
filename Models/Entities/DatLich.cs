using Doantotnghiep.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doantotnghiep.Models.Entities
{
    public class DatLich
    {
        [Key]
        public int IdDatLich { get; set; }
        [Required]
        public int IdKhachHang { get; set; }
        [ForeignKey("IdKhachHang")]
        public KhachHang KhachHang { get; set; }
        public int IdChiTietDatLich { get; set; }
        [ForeignKey("IdChiTietDatLich")]
        public ChiTietDatLich ChiTietDatLich { get; set; }
        [Required]
        public int IdNhanVien { get; set; }
        [ForeignKey("IdNhanVien")]
        public NhanVien? NhanVien { get; set; }
        [Required]
        public DateTime NgayHenLich { get; set; }
        [Required]
        public DateTime GioBatDauDV { get; set; }
        [Required]
        public DateTime GioKetThucDV { get; set; }
        [Required]
        public TrangThaiDatLich TrangThaiDatLich { get; set; }
        [Required]
        public string GhiChuDatLich { get; set; } = string.Empty;
        [Required]
        public DateTime NgayTaoDatLich { get; set; } = DateTime.Now;
        [Required]
        public DateTime? NgayCapNhatDatLich { get; set; } 
        public ICollection<HoaDon?> HoaDons { get; set; } 
        public ICollection<ChiTietDatLich?> ChiTietDatLichs { get; set; }

    }
}
