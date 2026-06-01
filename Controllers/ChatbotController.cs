using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Doantotnghiep.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly IChatbotAIService _chatbotAIService;
        public ChatbotController( IChatbotAIService chatbotAIService)
        {
            _chatbotAIService = chatbotAIService;
        }
        public async Task<IActionResult> Index(int? idHoiThoaiAI)
        {
            if (idHoiThoaiAI != null)
            {
                var model = await _chatbotAIService.LayHoiThoaiAsync(idHoiThoaiAI.Value);
                return View(model);
            }

            var idKhachHang = GetIdKhachHang();

            var hoiThoaiDangMo = await _chatbotAIService.LayHoiThoaiDangMoCuaKhachAsync(idKhachHang);

            if (hoiThoaiDangMo != null)
            {
                return View(hoiThoaiDangMo);
            }

            var modelMoi = await _chatbotAIService.BatDauChatAsync(idKhachHang);
            return View(modelMoi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuiTinNhan(int? idHoiThoaiAI, string cauHoi)
        {
            try
            {
                var model = await _chatbotAIService.GuiTinNhanAsync(idHoiThoaiAI, GetIdKhachHang(), cauHoi);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                if (idHoiThoaiAI != null)
                {
                    return RedirectToAction(nameof(Index), new { idHoiThoaiAI });
                }

                return RedirectToAction(nameof(Index));
            }
        }

        private int? GetIdKhachHang()
        {
            var claim = User.FindFirst("IdKhachHang")?.Value;

            if (int.TryParse(claim, out var idKhachHang))
            {
                return idKhachHang;
            }

            return null;
        }

        public async Task<IActionResult> TaoChatMoi()
        {
            var modelMoi = await _chatbotAIService.BatDauChatAsync(GetIdKhachHang());
            return RedirectToAction(nameof(Index), new { idHoiThoaiAI = modelMoi.IdHoiThoaiAI });
        }

    }
}
