using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Services.Interfaces
{
    public interface INhanVienServices
    {
        Task<List<NhanVien>> GetAllNhanVienAsync();
        Task<NhanVien> GetNhanVienByIdAsync(int id);
        Task<(bool ok, string error)> CreateNhanVienAsync(NhanVien nhanVien);
        Task<(bool ok, string error)> UpdateNhanVienAsync(NhanVien nhanVien);
        Task<(bool ok, string error)> DeleteNhanVienAsync(int id);
        Task<List<NhanVien>> GetNhanVienByChuyenMonAsync(string? chuyenMon);
        Task<List<NhanVien>> GetNhanVienByTrangThaiAsync(bool trangThai);
        Task<int> SaveChangeAsync();
    }
}
