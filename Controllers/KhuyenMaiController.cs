using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KhuyenMaiController : Controller
    {
        private readonly IKhuyenMaiService _khuyenMaiService;

        public KhuyenMaiController(IKhuyenMaiService khuyenMaiService)
        {
            _khuyenMaiService = khuyenMaiService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _khuyenMaiService.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var data = await _khuyenMaiService.GetByIdAsync(id);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _khuyenMaiService.GetCreateViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhuyenMaiViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DichVus = await _khuyenMaiService.GetDichVuSelectListAsync();
                return View(model);
            }

            try
            {
                await _khuyenMaiService.CreateAsync(model);
                TempData["SuccessMessage"] = "Tạo khuyến mãi thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                model.DichVus = await _khuyenMaiService.GetDichVuSelectListAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _khuyenMaiService.GetEditViewModelAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhuyenMaiViewModel model)
        {
            if (id != model.IdKhuyenMai)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                model.DichVus = await _khuyenMaiService.GetDichVuSelectListAsync();
                return View(model);
            }

            try
            {
                await _khuyenMaiService.UpdateAsync(model);
                TempData["SuccessMessage"] = "Cập nhật khuyến mãi thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                model.DichVus = await _khuyenMaiService.GetDichVuSelectListAsync();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _khuyenMaiService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Xóa khuyến mãi thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}