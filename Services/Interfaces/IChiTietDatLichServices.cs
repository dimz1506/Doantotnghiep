using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IChiTietDatLichServices
    {
        Task<List<ChiTietDatLich>> GetAllAsync();
        Task<List<ChiTietDatLich>> GetByDatLichIdAsync(int idDatLich);
        Task<ChiTietDatLich?> GetByIdAsync(int id);
        Task<bool> CreateAsync(ChiTietDatLich chiTietDatLich);
        Task<bool> UpdateAsync(ChiTietDatLich chiTietDatLich);
        Task<bool> DeleteAsync(int id);
    }
}
