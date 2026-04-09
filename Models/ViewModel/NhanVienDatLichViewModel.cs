using Doantotnghiep.Models.Enum;

namespace Doantotnghiep.Models.ViewModel
{
    public class NhanVienDatLichViewModel
    {
        public string TenNhanVien { get; set; } = string.Empty;
        public TrangThaiDatLich TrangThaiDatLich { get; set; }
        public DateTime NgayHenLich { get; set; }

    }
}
