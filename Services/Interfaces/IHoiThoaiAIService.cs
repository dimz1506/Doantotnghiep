using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IHoiThoaiAIService
    {
        Task<List<HoiThoaiAI>> GetHoiThoaiChoNhanVienAsync();
        Task<List<HoiThoaiAI>> GetHoiThoaiCuaNhanVienAsync(int idNhanVien);
        Task<List<HoiThoaiAI>> GetTatCaHoiThoaiAsync();
        Task<HoiThoaiAI> GetByIdAsync(int id);
        Task TiepNhanAsync(int idHoiThoaiAI, int idNhanVien);
        Task NhanVienTraLoiAsync(int idHoiThoaiAI, int idNhanVien, string noiDung);
        Task KetThucHoiThoaiAsync(int idHoiThoaiAI);
    }
}
