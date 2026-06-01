using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IHoaDonService
    {
        Task<List<HoaDon>> GetAllAsync();

        Task<HoaDon> GetByIdAsync(int id);

        Task<HoaDon?> GetByDatLichAsync(int idDatLich);

        Task<HoaDon> TaoHoaDonTuDatLichAsync(int idDatLich);

        Task ThanhToanAsync(int idHoaDon, PhuongThucTT phuongThucTT);
    }
}