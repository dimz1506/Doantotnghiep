using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doantotnghiep.Models.ViewModel
{
    public class ChiTietDatLichViewModel
    {
        public int IdChiTietDatLich { get; set; }
        public int IdDatLich { get; set; }
        public int IdDichVu {  get; set; }
        public string TenDichVu { get; set; } = string.Empty;
        public decimal GiaDichVu { get; set; }
        public int SoLuong {  get; set; }
        public decimal ThanhTien => GiaDichVu * SoLuong;
        public IEnumerable<SelectListItem> DichVus { get; set; } = new List<SelectListItem>();
    }
}
