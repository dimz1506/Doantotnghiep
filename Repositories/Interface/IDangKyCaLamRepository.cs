using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Repositories.Interface
{
    public interface IDangKyCaLamRepository
    {
        Task AddAsync(DangKyCaLamViec dangkycalam);
        Task<List<DangKyCaLamViec>> GetAllAsync();
        Task<DangKyCaLamViec?> GetByIdAsync(int id);
        Task UpdateAsync(DangKyCaLamViec suaDangKycaLam);
        Task<List<DangKyCaLamViec>> GetByNhanVienAsync(int idNhanVien);
    }
}
