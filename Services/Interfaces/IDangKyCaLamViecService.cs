using Doantotnghiep.Models.ViewModel;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IDangKyCaLamViecService
    {
        Task DangKyCaAsync(DangKyCaViewModel viewModel);
        Task<List<LichLamViecViewModel>> GetAllDangKyCaAsync();
        Task<List<LichLamViecViewModel>> GetDangKyByNhanVienAsync(int idNhanVien);
        Task<List<LichLamViecViewModel>> GetLichLamByNhanVienAsync(int idNhanVien);
        Task DuyetCaAsync(int id);
        Task TuChoiAsync(int id);

    }
}
 