using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Models.ViewModel
{
    public class DangKyCaViewModel
    {
        public int IdCaLamViec{ get; set; }
        public int IdNhanvien {  get; set; }
        public List<CaLamViec?> DanhSachCaLamViec {  get; set; } 

    }
}
