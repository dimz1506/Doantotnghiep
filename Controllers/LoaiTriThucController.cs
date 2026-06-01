using Doantotnghiep.Models.Entities;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LoaiTriThucController : Controller
    {
        private readonly ILoaiTriThucService _service;

        public LoaiTriThucController(ILoaiTriThucService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoaiTriThuc model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _service.AddAsync(model);

            TempData["SuccessMessage"] = "Thêm loại tri thức thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetByIdAsync(id);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoaiTriThuc model)
        {
            if (id != model.IdLoaiTriThuc)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _service.UpdateAsync(model);

            TempData["SuccessMessage"] = "Cập nhật loại tri thức thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            TempData["SuccessMessage"] = "Xóa loại tri thức thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}