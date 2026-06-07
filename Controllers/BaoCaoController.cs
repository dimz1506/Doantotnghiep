using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BaoCaoController : Controller
    {
        private readonly IThongKeService _thongKeService;
        public BaoCaoController(IThongKeService thongKeService)
        {
            _thongKeService = thongKeService;
        }

        public async Task<IActionResult> DoanhThuTheoNgay(DateTime? tuNgay, DateTime? denNgay)
        {
            var start = tuNgay ?? DateTime.Today.AddDays(-7);
            var end = denNgay ?? DateTime.Today;

            ViewBag.TuNgay = start.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = end.ToString("yyyy-MM-dd");

            var data = await _thongKeService.GetDoanhThuTheoNgayAsync(start, end);
            return View(data);
        }

        public async Task<IActionResult> DoanhThuTheoThang(int? nam)
        {
            var year = nam ?? DateTime.Today.Year;

            ViewBag.Nam = year;

            var data = await _thongKeService.GetDoanhThuTheoThangAsync(year);
            return View(data);
        }

        public async Task<IActionResult> DichVuBanChay(DateTime? tuNgay, DateTime? denNgay)
        {
            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");

            var data = await _thongKeService.GetDichVuBanChayAsync(tuNgay, denNgay);
            return View(data);
        }

        public async Task<IActionResult> LichHenTheoTrangThai()
        {
            var data = await _thongKeService.GetLichHenTheoTrangThaiAsync();
            return View(data);
        }

        public async Task<IActionResult> ThongKeNhanVien(DateTime? tuNgay, DateTime? denNgay)
        {
            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");

            var data = await _thongKeService.GetThongKeNhanVienAsync(tuNgay, denNgay);
            return View(data);
        }
    }
}
