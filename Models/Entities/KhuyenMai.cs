using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.Entities
{
    public class KhuyenMai
    {
        [Key]
        public int IdKhuyenMai { get; set; }
        [Required]
        public string TenKhuyenMai { get; set; } = string.Empty;
        [Required]
        public string MoTaKhuyenMai { get; set; } = string.Empty;
        [Required]
        public DateTime NgayBatDau { get; set; }
        [Required]
        public DateTime NgayKetThuc { get; set; }
        [Required]
        public decimal PhanTramGiamGia { get; set; }
        [Required]
        public bool TrangThaiKhuyenMai { get; set; }
        [Required]
        public DateTime NgayTaoKhuyenMai { get; set; } = DateTime.Now;
        [Required]
        public DateTime NgayCapNhatKhuyenMai { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        [Required]
        public DateTime? NgayXoaKhuyenMai { get; set; }
        public ICollection<DichVuKhuyenMai> DichVuKhuyenMais { get; set; }
        public ICollection<ChiTietHoaDon>  ChiTietHoaDons { get; set; }
    }
}
