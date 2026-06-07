using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services.Implementations;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Doantotnghiep.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly IKhachHangServices _khachHangServices;
        public KhachHangController(IKhachHangServices khachHangServices)
        {
            _khachHangServices = khachHangServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? searchString)
        {
            List<KhachHang> khachHangs;

            if (User.IsInRole("Admin") || User.IsInRole("NhanVien"))
            {
                khachHangs = await _khachHangServices.GetAllKhachHangAsync();

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    var keyword = searchString.Trim().ToLower();

                    khachHangs = khachHangs
                        .Where(kh =>
                            kh.TenKhachHang.ToLower().Contains(keyword)
                            || (!string.IsNullOrWhiteSpace(kh.DiaChiKhachHang)
                                && kh.DiaChiKhachHang.ToLower().Contains(keyword))
                            || (!string.IsNullOrWhiteSpace(kh.GhiChuKH)
                                && kh.GhiChuKH.ToLower().Contains(keyword))
                            || (kh.TaiKhoan != null
                                && kh.TaiKhoan.TenTaiKhoan.ToLower().Contains(keyword))
                            || kh.IdKhachHang.ToString().Contains(keyword)
                        )
                        .ToList();
                }
            }
            else
            {
                var idTaiKhoanClaim =
                    User.FindFirst("IdTaiKhoan")?.Value
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(idTaiKhoanClaim) ||
                    !int.TryParse(idTaiKhoanClaim, out int idTaiKhoan))
                {
                    return Unauthorized();
                }

                var khachHang = await _khachHangServices.GetKhachHangByTaiKhoanIdAsync(idTaiKhoan);

                if (khachHang == null)
                {
                    return NotFound();
                }

                khachHangs = new List<KhachHang> { khachHang };
            }

            ViewData["CurrentFilter"] = searchString;
            return View(khachHangs);
        } 

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var khachHang = await _khachHangServices.GetKhachHangByIdAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            if (User.IsInRole("KhachHang"))
            {
                var idTaiKhoanClaim = User.FindFirst("IdTaiKhoan")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(idTaiKhoanClaim) || !int.TryParse(idTaiKhoanClaim, out int idTaiKhoan))
                {
                    return Unauthorized();
                }
                if (khachHang.IdTaiKhoan != idTaiKhoan)
                {
                    return Forbid();
                }
            }
            return View(khachHang);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var khachhang = await _khachHangServices.GetKhachHangByIdAsync(id);
            if (khachhang == null)
            {
                return NotFound();
            }
            if (User.IsInRole("KhachHang"))
            {
                var idTaiKhoanClaim = User.FindFirst("IdTaiKhoan")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(idTaiKhoanClaim) || !int.TryParse(idTaiKhoanClaim, out int idTaiKhoan))
                {
                    return Unauthorized();
                }
                if (khachhang.IdTaiKhoan != idTaiKhoan)
                {
                    return Forbid();
                }
            }
            //map entity -> viewmodel
            var vm = new EditKhachHangViewModel
            {
                IdKhachHang = khachhang.IdKhachHang,
                TenKhachHang = khachhang.TenKhachHang,
                DiaChiKhachHang = khachhang.DiaChiKhachHang,
                GhiChuKH = khachhang.GhiChuKH,
                NgayTaoKH = khachhang.NgayTaoKH
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditKhachHangViewModel khachHang)
        {
            var khachhang1 = await _khachHangServices.GetKhachHangByIdAsync(khachHang.IdKhachHang);
            if (khachhang1 == null)
            {
                return NotFound();
            }
            if (User.IsInRole("KhachHang"))
            {
                var idTaiKhoanClaim = User.FindFirst("IdTaiKhoan")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(idTaiKhoanClaim) || !int.TryParse(idTaiKhoanClaim, out int idTaiKhoan))
                {
                    return Unauthorized();
                }
                if (khachhang1.IdTaiKhoan != idTaiKhoan)
                {
                    return Forbid();
                }
            }
            if (!ModelState.IsValid)
            {
                return View(khachHang);
            }
            //map viewmodel -> entity
            var khachHangToUpdate = new KhachHang
            {
                IdKhachHang = khachHang.IdKhachHang,
                TenKhachHang = khachHang.TenKhachHang,
                DiaChiKhachHang = khachHang.DiaChiKhachHang,
                GhiChuKH = khachHang.GhiChuKH,
                NgayTaoKH = khachHang.NgayTaoKH
            };
            var result = await _khachHangServices.UpdateKhachHangAsync(khachHangToUpdate);
            if (!result.ok)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "Cập nhật khach hang thành công!";
            return RedirectToAction("Index");
        }
        
        }
}
