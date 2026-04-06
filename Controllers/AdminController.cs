using Doantotnghiep.Data;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AuthService authService;
        public AdminController(AuthService authService)
        {
            this.authService = authService;
        }
        // GET: AdminController
        
        public IActionResult TaoNhanVien()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> TaoNhanVien(CreateNhanVienVM createNhanVienVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createNhanVienVM);
            }
            var result = await authService.CreateNhanVienAsync(createNhanVienVM);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("TaoNhanVien");
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(createNhanVienVM);
        }
    }
}
