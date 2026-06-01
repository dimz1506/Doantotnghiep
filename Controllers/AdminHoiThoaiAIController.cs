using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminHoiThoaiAIController : Controller
    {
        private readonly IHoiThoaiAIService _hoiThoaiService;

        public AdminHoiThoaiAIController(IHoiThoaiAIService hoiThoaiService)
        {
            _hoiThoaiService = hoiThoaiService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _hoiThoaiService.GetTatCaHoiThoaiAsync();
            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var data = await _hoiThoaiService.GetByIdAsync(id);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KetThuc(int id)
        {
            await _hoiThoaiService.KetThucHoiThoaiAsync(id);
            TempData["SuccessMessage"] = "Đã kết thúc hội thoại.";
            return RedirectToAction(nameof(Index));
        }
    }
}