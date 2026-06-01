using Doantotnghiep.Data;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Doantotnghiep.Services.Interfaces;
using System.Threading.Tasks;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AuthService authService;
        private readonly IThongKeService _thongKeService;
        public AdminController(AuthService authService, IThongKeService thongKeService)
        {
            this.authService = authService;
            _thongKeService = thongKeService;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var start7Days = today.AddDays(-7);
            var year = today.Year;

            ViewBag.DoanhThuTheoNgay = await _thongKeService.GetDoanhThuTheoNgayAsync(start7Days, today);
            ViewBag.DoanhThuTheoThang = await _thongKeService.GetDoanhThuTheoThangAsync(year);
            ViewBag.DichVuBanChay = await _thongKeService.GetDichVuBanChayAsync(null, null);
            ViewBag.ThongKeNhanVien = await _thongKeService.GetThongKeNhanVienAsync(null, null);

            return View();
        }

        // GET: AdminController

        public IActionResult TaoNhanVien()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> TaoNhanVien(CreateNhanVienVM createNhanVienVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createNhanVienVM);
            }
            var result = await authService.CreateNhanVienAsync(createNhanVienVM);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("TaoNhanVien");
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(createNhanVienVM);
        }
    }
}
