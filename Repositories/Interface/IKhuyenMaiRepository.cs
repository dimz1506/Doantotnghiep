using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IKhuyenMaiRepository
    {
        Task<List<KhuyenMai>> GetAllAsync();
        Task<KhuyenMai?> GetByIdAsync(int id);
        Task AddAsync(KhuyenMai khuyenMai);
        Task UpdateAsync(KhuyenMai khuyenMai);
        Task SoftDeleteAsync(KhuyenMai khuyenMai);
    }
}
