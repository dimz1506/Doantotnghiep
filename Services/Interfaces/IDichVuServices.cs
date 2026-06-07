using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.ViewModel;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IDichVuServices
    {
        Task<(bool ok, string error)> CreateDichVuAsync(DichVu dichVu);
        Task<(bool ok, string error)> UpdateDichVuAsync(DichVu dichVu);
        Task<(bool ok, string error)> DeleteDichVuAsync(int id);
        Task<List<DichVu>> GetAllDichVuAsync(string? searchString, int? idLoaiDichVu);
        Task<DichVu> GetDichVuByIdAsync(int id);
        Task<int> SaveChangesAsync();
        Task<List<DichVu>> GetDichVusByIdAsync(List<int> id);
        Task<List<DichVuKhachDatNhieuVM>> GetDichVuKhachDatNhieuAsync(int idKhachHang);
    }
}
