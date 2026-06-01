using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Repositories.Interface
{
    public interface ILoaiTriThucRepository
    {
        Task<List<LoaiTriThuc>> GetAllAsync();
        Task<LoaiTriThuc> GetByIdAsync(int id);
        Task AddAsync(LoaiTriThuc loaiTriThuc);
        Task UpdateAsync(LoaiTriThuc loaiTriThuc);
    }
}
