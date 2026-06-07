using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doantotnghiep.Controllers
{
    public class ChiTietDatLichController : Controller
    {
        private readonly IChiTietDatLichServices _chiTietDatLichServices;
        private readonly IDichVuServices _dichVuServices;
        private readonly IDatLichServices _datLichServices;
        private readonly IHoaDonService _hoaDonService;
        public ChiTietDatLichController(IChiTietDatLichServices chiTietDatLichServices, IDichVuServices dichVuServices, IDatLichServices datLichServices, IHoaDonService hoaDonService)
        {
            _chiTietDatLichServices = chiTietDatLichServices;
            _dichVuServices = dichVuServices;
            _datLichServices = datLichServices;
            _hoaDonService = hoaDonService;
        }
        private async Task LoadDichVuData(ChiTietDatLichViewModel model)
        {
            var dichVus = await _dichVuServices.GetAllDichVuAsync(null, null);

            model.DichVus = dichVus.Select(dv => new SelectListItem
            {
                Value = dv.IdDichVu.ToString(),
                Text = $"{dv.TenDichVu} - {dv.GiaDichVu:N0} VNĐ" 
            });
        }
        [HttpGet]

        public async Task<IActionResult> Index(int idDatLich)
        {
            var chitietdatlich = await _chiTietDatLichServices.GetByDatLichIdAsync(idDatLich);
            ViewBag.IdDatLich = idDatLich;
            var hoaDon = await _hoaDonService.GetByDatLichAsync(idDatLich);
            ViewBag.HoaDon = hoaDon;

            return View(chitietdatlich);
        }
        [HttpGet]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> Create(int idDatLich)
        {
            var datlich = await _datLichServices.GetDatLichByIdAsync(idDatLich);
            if (datlich == null)
            {
                return NotFound();
            }
            var model = new ChiTietDatLichViewModel
            {
                IdDatLich = idDatLich
            };
            await LoadDichVuData(model);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,KhachHang")]
        public async Task<IActionResult> Create(ChiTietDatLichViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDichVuData(model);
                return View(model);
            }
            var dichvu = await _dichVuServices.GetDichVuByIdAsync(model.IdDichVu);
            if (dichvu == null)
            {
                ModelState.AddModelError("", "Dich Vu khong ton tai.");
                await LoadDichVuData(model);
                return View(model);
            }
            var entity = new ChiTietDatLich
            {
                IdDatLich = model.IdDatLich,
                IdDichVu = model.IdDichVu,
                GiaDichVu = model.GiaDichVu,
                SoLuong = model.SoLuong
            };
            var result = await _chiTietDatLichServices.CreateAsync(entity);
            if (!result)
            {
                ModelState.AddModelError("", "Tao CHi tiet dat lich that bai.");
                await LoadDichVuData(model);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { idDatLich = model.IdDatLich });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,KhachHang")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _chiTietDatLichServices.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new ChiTietDatLichViewModel
            {
                IdChiTietDatLich = entity.IdChiTietDatLich,
                IdDatLich = entity.IdDatLich,
                IdDichVu = entity.IdDichVu,
                TenDichVu = entity.DichVu?.TenDichVu,
                GiaDichVu = entity.GiaDichVu,
                SoLuong = entity.SoLuong
            };
            await LoadDichVuData(model);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,KhachHang")]
        public async Task<IActionResult> Edit(int id, ChiTietDatLichViewModel model)
        {
            if (id != model.IdChiTietDatLich)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                await LoadDichVuData(model);
                return View(model);
            }
            var entity = await _chiTietDatLichServices.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            var dichVu = await _dichVuServices.GetDichVuByIdAsync(model.IdDichVu);
            if (dichVu == null)
            {
                ModelState.AddModelError("", "Dich vu khong ton tai.");
                await LoadDichVuData(model);
                return View(model);
            }
            entity.IdDichVu = model.IdDichVu;
            entity.GiaDichVu = model.GiaDichVu;
            entity.SoLuong = model.SoLuong;
            var result = await _chiTietDatLichServices.UpdateAsync(entity);
            if (!result)
            {
                ModelState.AddModelError("", "Cap nhat chi tiet dat lich that bai.");
                await LoadDichVuData(model);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { idDatLich = model.IdDatLich });
        }
        [HttpGet]
        [Authorize(Roles = "Admin,KhachHang")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _chiTietDatLichServices.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,KhachHang")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _chiTietDatLichServices.GetByIdAsync((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            int idDatLich = entity.IdDatLich;
            var result = await _chiTietDatLichServices.DeleteAsync(idDatLich);
            if (!result)
            {
                TempData["ErrorMessage"] = "Xoa that bai.";
                return RedirectToAction(nameof(Index), new { idDatLich });
            }
            TempData["SuccessMessage"] = "Xoa thanh cong.";
            return RedirectToAction(nameof(Index), new { idDatLich });
        }
    }
}
