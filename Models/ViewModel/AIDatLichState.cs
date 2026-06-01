namespace Doantotnghiep.Models.ViewModel
{
    public class AIDatLichState
    {
        public bool DangDatLich { get; set; } = false;
        public int? IdDichVu { get; set; }
        public string? TenDichVu { get; set; }
        public DateTime? NgayHen {  get; set; }
        public TimeSpan? GioBatDau { get; set; }
        public int? IdNhanVien { get; set; }
        public string? TenNhanVien { get; set; }
        public bool DangChoXacNhan { get; set; } = false;

        public bool DangChoXacNhanHuy { get; set; } = false;
        public int? IdDatLichCanHuy { get; set; }
    }
}
