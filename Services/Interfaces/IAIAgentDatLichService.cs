using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IAIAgentDatLichService
    {
        Task<bool> LaYeuCauDatLichAsync(string noiDung);

        Task<string> XuLyDatLichBangAIAsync(HoiThoaiAI hoiThoai, int? idKhachHang, string noiDungKhach);
    }
}
