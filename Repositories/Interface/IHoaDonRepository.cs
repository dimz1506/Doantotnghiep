using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Repositories.Interface
{
    public interface IHoaDonRepository
    {
        Task<HoaDon> AddAsync(HoaDon hoaDon);
        Task<List<HoaDon>> GetAllAsync();
        Task<HoaDon?> GetByIdAsync(int id);
        Task<HoaDon?> GetByDatLichAsync(int idDatLich);
        Task UpdateAsync(HoaDon hoaDon);

    }
}
