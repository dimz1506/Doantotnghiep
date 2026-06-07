using Doantotnghiep.Models.Entities;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TriThucController : Controller
    {
        private readonly ITriThucService _service;

        public TriThucController(ITriThucService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var data = await _service.GetByIdAsync(id);
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.LoaiTriThucs = await _service.GetLoaiTriThucSelectListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TriThuc model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LoaiTriThucs = await _service.GetLoaiTriThucSelectListAsync();
                return View(model);
            }

            await _service.AddAsync(model);

            TempData["SuccessMessage"] = "Thêm tri thức thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetByIdAsync(id);
            ViewBag.LoaiTriThucs = await _service.GetLoaiTriThucSelectListAsync();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TriThuc model)
        {
            if (id != model.IdTriThuc)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.LoaiTriThucs = await _service.GetLoaiTriThucSelectListAsync();
                return View(model);
            }

            await _service.UpdateAsync(model);

            TempData["SuccessMessage"] = "Cập nhật tri thức thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeletedAsync(id);

            TempData["SuccessMessage"] = "Xóa tri thức thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}