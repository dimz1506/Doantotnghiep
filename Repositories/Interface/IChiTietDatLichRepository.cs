using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Repositories.Interface
{
    public interface IChiTietDatLichRepository
    {
        Task<List<ChiTietDatLich>> GetAllAsync();
        Task<List<ChiTietDatLich>> GetByDatLichIdAsync(int idDatLich);
        Task<ChiTietDatLich?> GetByIdAsync(int id);
        Task AddAsync (ChiTietDatLich chiTietDatLich);
        Task UpdateAsync(ChiTietDatLich chiTietDatLich);
        Task DeleteAsync(ChiTietDatLich chiTietDatLich);
        Task SaveChangeAsync();
    }
}
