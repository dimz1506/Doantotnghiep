using Doantotnghiep.Models.Enum;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly IHoaDonService _hoaDonService;

        public HoaDonController(IHoaDonService hoaDonService)
        {
            _hoaDonService = hoaDonService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _hoaDonService.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var data = await _hoaDonService.GetByIdAsync(id);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoHoaDon(int idDatLich)
        {
            try
            {
                await _hoaDonService.TaoHoaDonTuDatLichAsync(idDatLich);
                TempData["SuccessMessage"] = "Tạo hóa đơn thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index", "DatLich");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThanhToan(int idHoaDon, PhuongThucTT phuongThucTT)
        {
            try
            {
                await _hoaDonService.ThanhToanAsync(idHoaDon, phuongThucTT);
                TempData["SuccessMessage"] = "Thanh toán hóa đơn thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
