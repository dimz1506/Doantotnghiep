using Doantotnghiep.Models.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.ViewModel
{
    public class KhuyenMaiViewModel
    {
        public int IdKhuyenMai { get; set; }
        [Required(ErrorMessage ="Vui long nhap ten khuyen mai.")]
        public string TenKhuyenMai {  get; set; } = string.Empty;

        public string? MoTaKhuyenMai { get; set; }

        [Required(ErrorMessage ="Vui long chon ngay bat dau.")]
        public DateTime NgayBatDau { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Vui long chon ngay ket thuc.")]
        public DateTime NgayKetThuc { get; set; } = DateTime.Today;

        [Required]
        public LoaiKhuyenMai LoaiKhuyenMai { get; set; } = LoaiKhuyenMai.GiamTheoPhanTram;

        [Required(ErrorMessage ="Vui long nhap gia tri giam.")]
        [Range(0.01, double.MaxValue, ErrorMessage ="Gia tri giam phai lon hon 0")]
        public decimal GiaTriGiam {  get; set; }

        public bool TrangThaiKhuyenMai { get; set; } = true;

        public List<int> IdDichVus { get; set; } = new List<int>();
        public IEnumerable<SelectListItem> DichVus { get; set; } = new List<SelectListItem>();

    }
}
