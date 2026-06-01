using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Repositories.Interface
{
    public interface ILichlamViecRepository
    {
        Task AddAsync(LichLamViecNhanVien lichLamViecNV);
        Task<List<LichLamViecNhanVien>> GetAllAsync();
        Task<List<LichLamViecNhanVien>> GetByNhanVienAsync(int idNhanVien);
        Task<LichLamViecNhanVien> GetByIdAsync(int id);
    }
}
