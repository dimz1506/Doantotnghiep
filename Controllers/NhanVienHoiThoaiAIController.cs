using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles = "NhanVien")]
    public class NhanVienHoiThoaiAIController : Controller
    {
        private readonly IHoiThoaiAIService _hoiThoaiService;

        public NhanVienHoiThoaiAIController(IHoiThoaiAIService hoiThoaiService)
        {
            _hoiThoaiService = hoiThoaiService;
        }

        public async Task<IActionResult> Index()
        {
            var idNhanVien = GetIdNhanVien();

            ViewBag.ChoTiepNhan = await _hoiThoaiService.GetHoiThoaiChoNhanVienAsync();
            ViewBag.CuaToi = await _hoiThoaiService.GetHoiThoaiCuaNhanVienAsync(idNhanVien);

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var data = await _hoiThoaiService.GetByIdAsync(id);
            var idNhanVien = GetIdNhanVien();

            if (data.IdNhanVien != null && data.IdNhanVien != idNhanVien)
            {
                return Forbid();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TiepNhan(int id)
        {
            var idNhanVien = GetIdNhanVien();

            try
            {
                await _hoiThoaiService.TiepNhanAsync(id, idNhanVien);
                TempData["SuccessMessage"] = "Tiếp nhận hội thoại thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TraLoi(int idHoiThoaiAI, string noiDung)
        {
            var idNhanVien = GetIdNhanVien();

            try
            {
                await _hoiThoaiService.NhanVienTraLoiAsync(idHoiThoaiAI, idNhanVien, noiDung);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id = idHoiThoaiAI });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KetThuc(int id)
        {
            await _hoiThoaiService.KetThucHoiThoaiAsync(id);
            TempData["SuccessMessage"] = "Đã kết thúc hội thoại.";
            return RedirectToAction(nameof(Index));
        }

        private int GetIdNhanVien()
        {
            var claim = User.FindFirst("IdNhanVien")?.Value;

            if (!int.TryParse(claim, out var idNhanVien))
            {
                throw new Exception("Tài khoản nhân viên chưa có IdNhanVien.");
            }

            return idNhanVien;
        }
    }
}