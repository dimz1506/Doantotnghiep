namespace Doantotnghiep.Models.ViewModel
{
    public class DichVuKhachDatNhieuVM
    {
        public int IdDichVu { get; set; }
        public string TenDichVu { get; set; } = string.Empty;
        public string? HinhAnhDichVu { get; set; }
        public decimal GiaDichVu { get; set; }
        public int ThoiLuongDV { get; set; }
        public int SoLanDat { get; set; }
    }
}