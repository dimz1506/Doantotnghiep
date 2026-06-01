using Doantotnghiep.Models.Enum;

namespace Doantotnghiep.Models.ViewModel
{
    public class LichLamViecViewModel
    {
        public int IdLichLamViec { get; set; }
        public int IdNhanVien { get; set; }
        public string TenNhanVien { get; set;}
        public DateTime NgayLamViec { get; set;}
        public DateTime GioBatDauLV { get;set;}
        public DateTime GioKetThucLV { get; set;}
        public TrangThaiDangKyCaLam TrangThaiDangKyCaLam {  get; set;}
    }
}
