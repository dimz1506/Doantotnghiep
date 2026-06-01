using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Services.Interfaces
{
    public interface ILichLamViecServices 
    {
        Task AddAsync(LichLamViecNhanVien lichLamViecNV);
        Task<List<LichLamViecNhanVien>> GetAllAsync();
        Task<List<LichLamViecNhanVien>> GetByNhanVienAsync(int idNhanVien);
        Task<LichLamViecNhanVien> GetByIdAsync(int id);
    }
}
