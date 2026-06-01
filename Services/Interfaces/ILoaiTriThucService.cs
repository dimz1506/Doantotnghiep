using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Services.Interfaces
{
    public interface ILoaiTriThucService
    {
        Task<List<LoaiTriThuc>> GetAllAsync();
        Task<LoaiTriThuc> GetByIdAsync(int id);
        Task AddAsync (LoaiTriThuc loaiTriThuc);
        Task UpdateAsync(LoaiTriThuc loaiTriThuc);
        Task DeleteAsync(int id);
    }
}
