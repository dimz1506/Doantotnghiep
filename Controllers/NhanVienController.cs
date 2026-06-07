using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Data;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Controllers
{
   
    public class NhanVienController : Controller
    {
        private readonly INhanVienServices _nhanVienServices;
        private readonly AppDbContext _context;
        public NhanVienController(INhanVienServices nhanVienServices, AppDbContext context)
        {
            _nhanVienServices = nhanVienServices;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> Index(string? searchString)
        {
            List<NhanVien> nhanViens;
            if(User.IsInRole("Admin"))
            {
                nhanViens = await _nhanVienServices.GetAllNhanVienAsync();
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    searchString = searchString.Trim().ToLower();

                    nhanViens = nhanViens.Where(nv =>
                        nv.TenNhanVien.ToLower().Contains(searchString)
                        || nv.ChuyenMonNV.ToLower().Contains(searchString)
                        || (nv.DiaChiNV != null && nv.DiaChiNV.ToLower().Contains(searchString))
                        || (nv.TaiKhoan != null && nv.TaiKhoan.TenTaiKhoan.ToLower().Contains(searchString))
                    ).ToList();
                }
            }
            else
            {
                var idTaiKhoanClaim = User.FindFirst("IdTaiKhoan")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(idTaiKhoanClaim) || !int.TryParse(idTaiKhoanClaim, out int idTaiKhoan))
                {
                    return Unauthorized();
                }
                var nhanvien = await _nhanVienServices.GetNhanVienByTaiKhoanIdAsync(idTaiKhoan);
                if (nhanvien == null)
                {
                    return NotFound();
                }
                nhanViens = new List<NhanVien> { nhanvien };
            }
            ViewData["CurrentFilter"] = searchString;
            return View(nhanViens);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> Details(int id)
        {
            var nhanvien = await _nhanVienServices.GetNhanVienByIdAsync(id);
            if (nhanvien == null)
            {
                return NotFound();
            }
            if(User.IsInRole("NhanVien"))
            {
                var idTaiKhoanClaim = User.FindFirst("IdTaiKhoan")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(idTaiKhoanClaim) || !int.TryParse(idTaiKhoanClaim, out int idTaiKhoan))
                {
                    return Unauthorized();
                }
                if (nhanvien.IdTaiKhoan != idTaiKhoan)
                {
                    return Forbid();
                }
            }
            return View(nhanvien);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> Edit(int id)
        {
            var nhanvien = await _context.NhanViens
                .Include(x => x.NhanVienDichVus)
                .FirstOrDefaultAsync(x => x.IdNhanVien == id);

            if (nhanvien == null)
            {
                return NotFound();
            }

            if (User.IsInRole("NhanVien"))
            {
                var idTaiKhoanClaim =
                    User.FindFirst("IdTaiKhoan")?.Value
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(idTaiKhoanClaim) ||
                    !int.TryParse(idTaiKhoanClaim, out int idTaiKhoan))
                {
                    return Unauthorized();
                }

                if (nhanvien.IdTaiKhoan != idTaiKhoan)
                {
                    return Forbid();
                }
            }

            var vm = new EditNhanVienVM
            {
                IdNhanVien = nhanvien.IdNhanVien,
                TenNhanVien = nhanvien.TenNhanVien,
                DiaChiNV = nhanvien.DiaChiNV,
                ChuyenMonNV = nhanvien.ChuyenMonNV,
                LaNhanVienFullTime = nhanvien.LaNhanVienFullTime,
                GioBatDauLamViec = nhanvien.GioBatDauLamViec,
                GioKetThucLamViec = nhanvien.GioKetThucLamViec,
                TrangThaiNV = nhanvien.TrangThaiNV,
                IdDichVus = nhanvien.NhanVienDichVus?
                    .Select(x => x.IdDichVu)
                    .ToList() ?? new List<int>()
            };

            await LoadEditNhanVienData(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> Edit(EditNhanVienVM model)
        {
            var nhanvien = await _context.NhanViens
                .Include(x => x.NhanVienDichVus)
                .FirstOrDefaultAsync(x => x.IdNhanVien == model.IdNhanVien);

            if (nhanvien == null)
            {
                return NotFound();
            }

            var laAdmin = User.IsInRole("Admin");

            if (User.IsInRole("NhanVien"))
            {
                var idTaiKhoanClaim =
                    User.FindFirst("IdTaiKhoan")?.Value
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(idTaiKhoanClaim) ||
                    !int.TryParse(idTaiKhoanClaim, out int idTaiKhoan))
                {
                    return Unauthorized();
                }

                if (nhanvien.IdTaiKhoan != idTaiKhoan)
                {
                    return Forbid();
                }

                ModelState.Remove(nameof(model.ChuyenMonNV));
                ModelState.Remove(nameof(model.TrangThaiNV));
                ModelState.Remove(nameof(model.IdDichVus));
            }

            if (laAdmin && model.GioBatDauLamViec >= model.GioKetThucLamViec)
            {
                ModelState.AddModelError("GioKetThucLamViec", "Giờ kết thúc phải lớn hơn giờ bắt đầu.");
            }

            if (laAdmin && (model.IdDichVus == null || !model.IdDichVus.Any()))
            {
                ModelState.AddModelError("IdDichVus", "Vui lòng chọn ít nhất một dịch vụ nhân viên phụ trách.");
            }

            if (!ModelState.IsValid)
            {
                await LoadEditNhanVienData(model);
                return View(model);
            }

            nhanvien.TenNhanVien = model.TenNhanVien;
            nhanvien.DiaChiNV = model.DiaChiNV ?? string.Empty;

            if (laAdmin)
            {
                nhanvien.ChuyenMonNV = model.ChuyenMonNV;
                nhanvien.LaNhanVienFullTime = model.LaNhanVienFullTime;
                nhanvien.GioBatDauLamViec = model.GioBatDauLamViec;
                nhanvien.GioKetThucLamViec = model.GioKetThucLamViec;
                nhanvien.TrangThaiNV = model.TrangThaiNV;

                var dichVuCu = _context.NhanVienDichVus
                    .Where(x => x.IdNhanVien == nhanvien.IdNhanVien);

                _context.NhanVienDichVus.RemoveRange(dichVuCu);

                foreach (var idDichVu in model.IdDichVus.Distinct())
                {
                    _context.NhanVienDichVus.Add(new NhanVienDichVu
                    {
                        IdNhanVien = nhanvien.IdNhanVien,
                        IdDichVu = idDichVu
                    });
                }
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật nhân viên thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(string? chuyenMon, TrangThaiNhanVien? trangThai)
        {
            var nhanviens = await _nhanVienServices.GetAllNhanVienAsync();
            if(!string.IsNullOrEmpty(chuyenMon))
            {
                nhanviens = nhanviens.Where(nv => nv.ChuyenMonNV.Contains(chuyenMon)).ToList();
            }
            if (trangThai.HasValue)
            {
                nhanviens = nhanviens.Where(nv => nv.TrangThaiNV == (trangThai.Value == TrangThaiNhanVien.DangLamViec ? Models.Enum.TrangThaiNhanVien.DangLamViec : Models.Enum.TrangThaiNhanVien.NghiViec)).ToList();
            }
            return View("Index", nhanviens);
        }
        private async Task LoadEditNhanVienData(EditNhanVienVM model)
        {
            var dichVus = await _context.DichVus
                .Where(x => !x.IsDeleted && x.TrangThaiDV)
                .OrderBy(x => x.TenDichVu)
                .Select(x => new SelectListItem
                {
                    Value = x.IdDichVu.ToString(),
                    Text = x.TenDichVu
                })
                .ToListAsync();

            model.DichVus = dichVus;
        }

    }
}
