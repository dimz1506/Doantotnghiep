using Doantotnghiep.Data;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Doantotnghiep.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AuthService authService;
        private readonly IThongKeService _thongKeService;
        private readonly IDichVuServices _dichVuServices;

        public AdminController(
            AuthService authService,
            IThongKeService thongKeService,
            IDichVuServices dichVuServices)
        {
            this.authService = authService;
            _thongKeService = thongKeService;
            _dichVuServices = dichVuServices;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var start7Days = today.AddDays(-7);
            var year = today.Year;

            ViewBag.DoanhThuTheoNgay = await _thongKeService.GetDoanhThuTheoNgayAsync(start7Days, today);
            ViewBag.DoanhThuTheoThang = await _thongKeService.GetDoanhThuTheoThangAsync(year);
            ViewBag.DichVuBanChay = await _thongKeService.GetDichVuBanChayAsync(null, null);
            ViewBag.ThongKeNhanVien = await _thongKeService.GetThongKeNhanVienAsync(null, null);

            return View();
        }

        // GET: AdminController

        public async Task<IActionResult> TaoNhanVien()
        {
            var model = new CreateNhanVienVM();
            await LoadTaoNhanVienData(model);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoNhanVien(CreateNhanVienVM createNhanVienVM)
        {
            if (createNhanVienVM.IdDichVus == null || !createNhanVienVM.IdDichVus.Any())
            {
                ModelState.AddModelError("IdDichVus", "Vui lòng chọn ít nhất một dịch vụ nhân viên phụ trách.");
            }

            if (createNhanVienVM.GioBatDauLamViec >= createNhanVienVM.GioKetThucLamViec)
            {
                ModelState.AddModelError("GioKetThucLamViec", "Giờ kết thúc phải lớn hơn giờ bắt đầu.");
            }

            if (!ModelState.IsValid)
            {
                await LoadTaoNhanVienData(createNhanVienVM);
                return View(createNhanVienVM);
            }

            var result = await authService.CreateNhanVienAsync(createNhanVienVM);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("TaoNhanVien");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            await LoadTaoNhanVienData(createNhanVienVM);
            return View(createNhanVienVM);
        }

        private async Task LoadTaoNhanVienData(CreateNhanVienVM model)
        {
            var dichVus = await _dichVuServices.GetAllDichVuAsync(null, null);

            model.DichVus = dichVus
                .Where(x => !x.IsDeleted && x.TrangThaiDV)
                .Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = x.IdDichVu.ToString(),
                    Text = x.TenDichVu
                })
                .ToList();
        }
    }
}
