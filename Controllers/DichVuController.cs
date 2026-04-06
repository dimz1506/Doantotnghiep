using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doantotnghiep.Controllers
{
    public class DichVuController : Controller
    {
        private readonly IDichVuServices _dichVuServices;
        private readonly ILoaiDichVuServices _loaiDichVuServices;
        public DichVuController(IDichVuServices dichVuServices, ILoaiDichVuServices loaiDichVuServices)
        {
            _dichVuServices = dichVuServices;
            _loaiDichVuServices = loaiDichVuServices;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchString)
        {
            var dichVus = await _dichVuServices.GetAllDichVuAsync(searchString);
            ViewBag.SearchString = searchString;
            return View(dichVus);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> Create()
        {
            var loaiDichVus = await _loaiDichVuServices.GetAllLoaiDichVuAsync(null);
            ViewBag.LoaiDichVus = new SelectList(loaiDichVus, "IdLoaiDichVu", "TenLoaiDichVu");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,NhanVien")]

        public async Task<IActionResult> Create(DichVu dichVu)
        {
            if(dichVu.HinhAnhFile == null || dichVu.HinhAnhFile.Length == 0)
            {
                ModelState.AddModelError("HinhAnhFile", "Vui lòng chọn hình ảnh cho dịch vụ.");
            }
            foreach (var key in ModelState.Keys)
            {
                foreach (var error in ModelState[key].Errors)
                {
                    Console.WriteLine($"KEY: {key} | ERROR: {error.ErrorMessage}");
                }
            }
            if (!ModelState.IsValid)
            {
                var loaiDichVus = await _loaiDichVuServices.GetAllLoaiDichVuAsync(null);
                ViewBag.LoaiDichVus = new SelectList(loaiDichVus, "IdLoaiDichVu", "TenLoaiDichVu", dichVu.IdLoaiDichVu);
                return View(dichVu);
            }
            //xu ly upload anh 
            if(dichVu.HinhAnhFile != null && dichVu.HinhAnhFile.Length > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "dichvu");
                Directory.CreateDirectory(uploadDir);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dichVu.HinhAnhFile.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dichVu.HinhAnhFile.CopyToAsync(stream);
                }
                dichVu.HinhAnhDichVu = "/images/dichvu/" + fileName;
            }
            dichVu.NgayTaoDichVu = DateTime.Now;
            var result = await _dichVuServices.CreateDichVuAsync(dichVu);
            if (!result.ok)
            {
                TempData["ErrorMessage"] = result.error;
                var loaiDichvus = await _loaiDichVuServices.GetAllLoaiDichVuAsync(null);
                ViewBag.LoaiDichVus = new SelectList(loaiDichvus, "IdLoaiDichVu", "TenLoaiDichVu", dichVu.IdLoaiDichVu);
                return View(dichVu);
            }
           
            TempData["SuccessMessage"] = "Thêm dịch vụ thành công.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin,NhanVien")]

        public async Task<IActionResult> Edit(int id)
        {
            var dichVu = await _dichVuServices.GetDichVuByIdAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            var loaiDichVus = await _loaiDichVuServices.GetAllLoaiDichVuAsync(null);
            ViewBag.LoaiDichVus = new SelectList(loaiDichVus, "IdLoaiDichVu", "TenLoaiDichVu", dichVu.IdLoaiDichVu);
            return View(dichVu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,NhanVien")]

        public async Task<IActionResult> Edit(DichVu dichVu)
        {
            if (!ModelState.IsValid)
            {
                var loaiDichVus = await _loaiDichVuServices.GetAllLoaiDichVuAsync(null);
                ViewBag.LoaiDichVus = new SelectList(loaiDichVus, "IdLoaiDichVu", "TenLoaiDichVu", dichVu.IdLoaiDichVu);
                return View(dichVu);
            }
            //xu ly up;oad anh
            if(dichVu.HinhAnhFile != null && dichVu.HinhAnhFile.Length > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "dichvu");
                Directory.CreateDirectory(uploadDir);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dichVu.HinhAnhFile.FileName);
                var filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dichVu.HinhAnhFile.CopyToAsync(stream);
                }
                dichVu.HinhAnhDichVu = "/images/dichvu/" + fileName;
            }
            var result = await _dichVuServices.UpdateDichVuAsync(dichVu);
            if (!result.ok)
            {
                TempData["ErrorMessage"] = result.error;
                var loaiDichVus = await _loaiDichVuServices.GetAllLoaiDichVuAsync(null);
                ViewBag.LoaiDichVus = new SelectList(loaiDichVus, "IdLoaiDichVu", "TenLoaiDichVu", dichVu.IdLoaiDichVu);
                return View(dichVu);
            }
            TempData["SuccessMessage"] = "Cập nhật dịch vụ thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,NhanVien")]

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dichVuServices.DeleteDichVuAsync(id);
            if (!result.ok)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Xóa dịch vụ thành công.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var dichVu = await _dichVuServices.GetDichVuByIdAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }
    }
}
