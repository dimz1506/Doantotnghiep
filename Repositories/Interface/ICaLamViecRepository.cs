using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Repositories.Interface
{
    public interface ICaLamViecRepository
    {
        Task AddAsync(CaLamViec caLamViec);
        Task<List<CaLamViec>> GetAllAsync();
        Task<List<CaLamViec>> GetCaDangMoAsync();
        Task<CaLamViec?> GetByIdAsync(int id);
        Task UpdateAsync(CaLamViec caLamViec);
    }
}
