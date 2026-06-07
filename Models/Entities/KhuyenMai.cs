using Doantotnghiep.Models.Enum;
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
        public LoaiKhuyenMai LoaiKhuyenMai { get; set; } = LoaiKhuyenMai.GiamTheoPhanTram;
        [Required]
        public decimal GiaTriGiam { get; set; }
        [Required]
        public bool TrangThaiKhuyenMai { get; set; }
        [Required]
        public DateTime NgayTaoKhuyenMai { get; set; } = DateTime.Now;
        public DateTime? NgayCapNhatKhuyenMai { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        public DateTime? NgayXoaKhuyenMai { get; set; }
        public ICollection<DichVuKhuyenMai> DichVuKhuyenMais { get; set; } = new List<DichVuKhuyenMai>();
        public ICollection<ChiTietHoaDon>  ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
    }
}
