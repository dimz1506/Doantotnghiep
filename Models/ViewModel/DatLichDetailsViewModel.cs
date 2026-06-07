using Doantotnghiep.Models.Enum;

namespace Doantotnghiep.Models.ViewModel
{
    public class DatLichDetailsViewModel
    {
        public int IdDatLich { get; set; }
        public int IdKhachHang { get; set; }
        public string TenKhachHang { get; set; } = string.Empty;

        public int IdNhanVien {  get; set; }
        public string TenNhanVien { get; set; } = string.Empty;

        public DateTime NgayHenLich { get; set; }
        public DateTime GioBatDauDV { get; set; }
        public DateTime GioKetThucDV { get; set; }

        public TrangThaiDatLich TrangThaiDatLich { get; set; }
        public string GhiChuDatLich { get; set; } = string.Empty;

        public DateTime NgayTaoDatLich { get; set; }
        public DateTime? NgayCapNhatDatLich { get; set; }

        public List<ChiTietDatLichViewModel> ChiTietDatLichs { get; set; } = new();
        public decimal TongTien => ChiTietDatLichs.Sum(x => x.ThanhTien);
    }
}
