using Doantotnghiep.Models.ViewModel;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IChatbotAIService
    {
        Task<ChatbotViewModel> BatDauChatAsync(int? idKhachHang);
        Task<ChatbotViewModel> GuiTinNhanAsync(int? idHoiThoaiAI, int? idKhachHang, string caHoi);
        Task<ChatbotViewModel> LayHoiThoaiAsync(int idHoiThoaiAI);

        Task<ChatbotViewModel?> LayHoiThoaiDangMoCuaKhachAsync(int? idKhachHang);
    }
}
