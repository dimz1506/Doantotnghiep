using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Services.Interfaces
{
    public interface ILoaiDichVuServices
    {
        Task<List<LoaiDichVu>> GetAllLoaiDichVuAsync(string? searchString);
        Task<LoaiDichVu> GetLoaiDichVuByIdAsync(int id);
        Task<(bool ok, string error)> CreateLoaiDichVuAsync(LoaiDichVu loaiDichVu);
        Task<(bool ok, string error)> UpdateLoaiDichVuAsync(LoaiDichVu loaiDichVu);
        Task<(bool ok, string error)> DeleteLoaiDichVuAsync(int id);
        Task<int> SaveChangesAsync();   
    }
}
