using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Repositories.Interface
{
    public interface IDatLichRepository
    {
        Task<List<DatLich>> GetAllDatLichsAsync();
        Task<DatLich?> GetDatLichByIdAsync(int id);
        Task<DatLich> CreateDatLichAsync(DatLich datLich);
        Task<DatLich?> UpdateDatLichAsync(int id, DatLich datLich);
        Task<(bool ok, string error)> DeleteDatLichAsync(int id);
    }
}
