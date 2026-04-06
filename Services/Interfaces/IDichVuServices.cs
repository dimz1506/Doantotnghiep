using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IDichVuServices
    {
        Task<(bool ok, string error)> CreateDichVuAsync(DichVu dichVu);
        Task<(bool ok, string error)> UpdateDichVuAsync(DichVu dichVu);
        Task<(bool ok, string error)> DeleteDichVuAsync(int id);
        Task<List<DichVu>> GetAllDichVuAsync(string? searchString);
        Task<DichVu> GetDichVuByIdAsync(int id);
        Task<int> SaveChangesAsync();

    }
}
