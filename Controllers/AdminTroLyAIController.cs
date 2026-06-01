using Doantotnghiep.Models.ViewModel.AdminAI;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTroLyAIController : Controller
    {
        private readonly IAdminAIService _adminAIService;

        public AdminTroLyAIController(IAdminAIService adminAIService)
        {
            _adminAIService = adminAIService;
        }

        public IActionResult Index()
        {
            return View(new AdminAIChatViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AdminAIChatViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CauHoi))
            {
                model.CauTraLoi = "Vui lòng nhập câu hỏi.";
                return View(model);
            }

            model.CauTraLoi = await _adminAIService.TraLoiAdminAsync(model.CauHoi);
            return View(model);
        }
    }
}