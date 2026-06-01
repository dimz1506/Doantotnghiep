using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Services.Interfaces
{
    public interface ICaLamViecService
    {
        Task AddAsync(CaLamViec caLamViec);
        Task<List<CaLamViec>> GetAllAsync();
        Task<List<CaLamViec>> GetCaDangMoAsync();
        Task<CaLamViec> GetByIdAsync(int id);
        Task UpdateAsync(CaLamViec caLamViec);
    }
}
