using Doantotnghiep.Models.Entities;
using Doantotnghiep.Services.Implementations;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    public class AdminLichLamViecController : Controller
    {
        private readonly IDangKyCaLamViecService _service;
        private readonly ILichLamViecServices _lichlamviecService;
        private readonly ICaLamViecService _caLamViecService;
        public AdminLichLamViecController(IDangKyCaLamViecService service, ILichLamViecServices lichLamViecServices, ICaLamViecService caLamViecService)
        {
            _service = service;
            _lichlamviecService = lichLamViecServices;
            _caLamViecService = caLamViecService;
        }
        public async Task<IActionResult> Index()
        {
            var chitietdatlich = await _lichlamviecService.GetAllAsync();
            
            return View(chitietdatlich);
        }
        public async Task<IActionResult> DanhSachCaLam()
        {
            var data = await _caLamViecService.GetAllAsync();
            return View(data);
        }

        public IActionResult TaoCaLam()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoCaLam(CaLamViec caLamViec)
        {
            if (!ModelState.IsValid)
            {
                return View(caLamViec);
            }

            await _caLamViecService.AddAsync(caLamViec);

            TempData["SuccessMessage"] = "Tạo ca làm thành công.";
            return RedirectToAction(nameof(DanhSachCaLam));
        }
        //danh sach dang ky ca
        public async Task<IActionResult> DanhSachDangKyca()
        {
            var data = await _service.GetAllDangKyCaAsync();
            return View(data);
        }
        //duyet ca
        public async Task<IActionResult> DuyetCa(int id)
        {
            await _service.DuyetCaAsync(id);
            TempData["SuccessMessage"] = "Duyet ca thanh cong.";
            return RedirectToAction(nameof(DanhSachDangKyca));
        }
        //tu choi 
        public async Task<IActionResult> TuChoi(int id)
        {
            await _service.TuChoiAsync(id);
            TempData["SuccessMessage"] = "Tu choi ca thanh cong.";
            return RedirectToAction(nameof(DanhSachDangKyca));
        }
        //danh sach lich lam viec
        public async Task<IActionResult> DanhSachLichLam()
        {
            var data = await _lichlamviecService.GetAllAsync();
            return View(data);

        }
    }
}
