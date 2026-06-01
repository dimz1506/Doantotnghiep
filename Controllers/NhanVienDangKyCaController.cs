using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Doantotnghiep.Controllers
{
    public class NhanVienDangKyCaController : Controller
    {
        private readonly IDangKyCaLamViecService _dangKyCaLamViecService;
        private readonly ICaLamViecService _caLamViecService;
        public NhanVienDangKyCaController(IDangKyCaLamViecService service, ICaLamViecService caLamViecService)
        {
            _dangKyCaLamViecService = service;
            _caLamViecService = caLamViecService;
        }
        // Nhân viên xem danh sách ca đang mở
        public async Task<IActionResult> Index()
        {
            var data = await _caLamViecService.GetCaDangMoAsync();
            return View(data);
        }

        // Nhân viên bấm đăng ký ca
        // Nhân viên bấm đăng ký ca
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKyCa(int idCaLamViec)
        {
            var idNhanVienClaim = User.FindFirst("IdNhanVien")?.Value;

            if (!int.TryParse(idNhanVienClaim, out var idNhanVien))
            {
                TempData["ErrorMessage"] = "Không xác định được nhân viên đang đăng nhập.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new DangKyCaViewModel
            {
                IdNhanvien = idNhanVien,
                IdCaLamViec = idCaLamViec
            };

            await _dangKyCaLamViecService.DangKyCaAsync(viewModel);

            TempData["SuccessMessage"] = "Đăng ký ca thành công, vui lòng chờ admin duyệt.";
            return RedirectToAction(nameof(Index));
        }

        // Nhân viên xem lịch sử đăng ký ca của mình
        public async Task<IActionResult> LichSuDangKy()
        {
            // Tạm thời fix cứng IdNhanVien để test
            int idNhanVien = 1;

            var data = await _dangKyCaLamViecService.GetDangKyByNhanVienAsync(idNhanVien);
            return View(data);
        }

        // Nhân viên xem lịch làm chính thức sau khi được duyệt
        public async Task<IActionResult> LichLamCuaToi()
        {
            // Tạm thời fix cứng IdNhanVien để test
            int idNhanVien = 1;

            var data = await _dangKyCaLamViecService.GetLichLamByNhanVienAsync(idNhanVien);
            return View(data);
        }

    }
}
