using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.IRepository;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles = "Admin,NhanVien")]
    public class LoaiDichVuController : Controller
    {
        private readonly ILoaiDichVuServices _loaiDichVuServices;
        private readonly AppDbContext _context;
        public LoaiDichVuController(ILoaiDichVuServices loaiDichVuServices, AppDbContext context)
        {
            _loaiDichVuServices = loaiDichVuServices;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchString)
        {
            var loaiDichVus = await _loaiDichVuServices.GetAllLoaiDichVuAsync(searchString);
            ViewBag.SearchString = searchString;
            return View(loaiDichVus);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoaiDichVu loaiDichVu)
        {
            if (!ModelState.IsValid)
            {
                return View(loaiDichVu);
            }
            loaiDichVu.NgayTaoLoaiDichVu = DateTime.Now;
            var result = await _loaiDichVuServices.CreateLoaiDichVuAsync(loaiDichVu);
            if (!result.ok)
            {
                TempData["ErrorMessage"] = result.error;
                return View(loaiDichVu);
            }
            TempData["SuccessMessage"] = "Thêm loại dịch vụ thành công.";
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var loaiDichVu = await _loaiDichVuServices.GetLoaiDichVuByIdAsync(id);
            if (loaiDichVu == null)
            {
                return NotFound();
            }
            return View(loaiDichVu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LoaiDichVu loaiDichVu)
        {
            if (!ModelState.IsValid)
            {
                return View(loaiDichVu);
            }
            var result = await _loaiDichVuServices.UpdateLoaiDichVuAsync(loaiDichVu);
            if (!result.ok)
            {
                TempData["ErrorMessage"] = result.error;
                return View(loaiDichVu);
            }
            TempData["SuccessMessage"] = "Cập nhật loại dịch vụ thành công.";
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loaiDichVuServices.DeleteLoaiDichVuAsync(id);
            if (!result.ok)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Xóa loại dịch vụ thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
