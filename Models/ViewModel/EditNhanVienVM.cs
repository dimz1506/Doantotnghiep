using Doantotnghiep.Models.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doantotnghiep.Models.ViewModel
{
    public class EditNhanVienVM
    {
        public int IdNhanVien { get; set; }

        public string TenNhanVien { get; set; } = string.Empty;

        public string? DiaChiNV { get; set; }

        public string ChuyenMonNV { get; set; } = string.Empty;

        public bool LaNhanVienFullTime { get; set; } = true;

        public TimeSpan GioBatDauLamViec { get; set; } = new TimeSpan(8, 0, 0);

        public TimeSpan GioKetThucLamViec { get; set; } = new TimeSpan(17, 0, 0);

        public TrangThaiNhanVien TrangThaiNV { get; set; }

        public List<int> IdDichVus { get; set; } = new();

        public IEnumerable<SelectListItem> DichVus { get; set; } = new List<SelectListItem>();
    }
}