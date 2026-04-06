using Doantotnghiep.Models.Entities;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Doantotnghiep.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly INhanVienServices _nhanVienServices;
        public NhanVienController(INhanVienServices nhanVienServices)
        {
            _nhanVienServices = nhanVienServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var nhanvien = await _nhanVienServices.GetAllNhanVienAsync();
            ViewBag.NhanVienCount = nhanvien.Count;
            return View(nhanvien);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var nhanvien = await _nhanVienServices.GetNhanVienByIdAsync(id);
            if (nhanvien == null)
            {
                return NotFound();
            }
            return View(nhanvien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _nhanVienServices.DeleteNhanVienAsync(id);
            if (!result.ok)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Xóa nhân viên thành công.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var nhanVien = await _nhanVienServices.GetAllNhanVienAsync();
            ViewBag.NhanVien = new SelectList(nhanVien, "IdNhanVien");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(NhanVien nhanVien)
        {
            if (!ModelState.IsValid)
            {
                var nhanVienList = await _nhanVienServices.GetAllNhanVienAsync();
                ViewBag.NhanVien = new SelectList(nhanVienList, "IdNhanVien", "TenNhanVien", nhanVien.IdNhanVien);
                return View(nhanVien);
            }
            var result = await _nhanVienServices.CreateNhanVienAsync(nhanVien);
            if (!result.ok)
            {
                TempData["ErrorMessage"] = result.error;
                var nhanVienList = await _nhanVienServices.GetAllNhanVienAsync();
                ViewBag.NhanVien = new SelectList(nhanVienList, "IdNhanVien", "TenNhanVien", nhanVien.IdNhanVien);
                return View(nhanVien);
            }
            TempData["SuccessMessage"] = "Thêm nhân viên thành công.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var nhanvien = await _nhanVienServices.GetNhanVienByIdAsync(id);
            if (nhanvien == null)
            {
                return NotFound();
            }
            return View(nhanvien);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(NhanVien nhanVien)
        {
            if (!ModelState.IsValid)
            {
                return View(nhanVien);
            }
            var result = await _nhanVienServices.UpdateNhanVienAsync(nhanVien);
            if (!result.ok)
            {
                TempData["ErrorMessage"] = result.error;
                return View(nhanVien);
            }
            TempData["SuccessMessage"] = "Cập nhật nhân viên thành công.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Search(string? chuyenMon, bool? trangThai)
        {
            List<NhanVien> nhanviens = new List<NhanVien>();
            if (!string.IsNullOrEmpty(chuyenMon))
            {
                nhanviens = await _nhanVienServices.GetNhanVienByChuyenMonAsync(chuyenMon);
            }
            else if (trangThai.HasValue)
            {
                nhanviens = await _nhanVienServices.GetNhanVienByTrangThaiAsync(trangThai.Value);
            }
            else
            {
                nhanviens = await _nhanVienServices.GetAllNhanVienAsync();
            }
            return View("Index", nhanviens);
        }
        [HttpPost]
        public async Task<IActionResult> SaveChange()
        {
            var result = await _nhanVienServices.SaveChangeAsync();
            if (result < 0)
            {
                return BadRequest("Lưu thay đổi thất bại.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
