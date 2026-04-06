using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Repositories.Interface
{
    public interface IDichVuRepository
    {
        Task<(bool ok, string error)> CreateDichVuAsync(DichVu dichVu);
        Task<(bool ok, string error)> UpdateDichVuAsync(DichVu dichVu);
        Task<(bool ok, string error)> DeleteDichVuAsync(int id);
        Task<List<DichVu>> GetAllDichVuAsync(string? searchString);
        Task<DichVu> GetDichVuByIdAsync(int id);
        Task<int> SaveChangesAsync();
    }
}
