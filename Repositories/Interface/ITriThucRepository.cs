using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Repositories.Interface
{
    public interface ITriThucRepository
    {
        Task<List<TriThuc>> GetAllAsync();
        Task<TriThuc?> GetByIdAsync(int id);
        Task AddAsync(TriThuc entity);
        Task UpdateAsync(TriThuc entity);
    }
}
