using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.ViewModel
{
    public class CreateDatLichViewModel
    {
       
        [Required(ErrorMessage = "Vui lòng chọn nhân viên!")]
        public int IdNhanVien { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ngày hẹn lịch!")]
        public DateTime NgayHenLich { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn giờ bắt đầu dịch vụ!")]
        public TimeSpan GioBatDauDV { get; set; } //su dung timespan de nguoi dung khong can nhap ngay chi can nhap gio phut
        
        [Required(ErrorMessage = "Vui lòng chọn dịch vụ!")]
        public List<int> IdDichVu { get; set; } = new List<int>(); // su dung list vi 1 lich co nhieu dich vu

        public string GhiChuDatLich { get; set; } = string.Empty;
       // public IEnumerable<SelectListItem>? KhachHangs { get; set; }
        public IEnumerable<SelectListItem>? NhanViens { get; set; } = new List<SelectListItem>();
        public IEnumerable<DichVuSelectItem>? DichVus { get; set; } = new List<DichVuSelectItem>();
    }
}
