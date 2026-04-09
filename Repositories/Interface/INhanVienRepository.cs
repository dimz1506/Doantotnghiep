using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;

namespace Doantotnghiep.Repositories.Interface
{
    public interface INhanVienRepository
    {
        Task<List<NhanVien>> GetAllNhanVienAsync();
        Task<NhanVien?> GetNhanVienByIdAsync(int id);
        Task<(bool ok, string error)> UpdateNhanVienAsync(NhanVien nhanVien);
        Task<List<NhanVien>> GetNhanVienByChuyenMonAsync(string? chuyenMon);
        Task<NhanVien?> GetNhanVienByTaiKhoanIdAsync(int taiKhoanId);
        Task<List<NhanVien>> GetNhanVienByTrangThaiAsync(TrangThaiNhanVien trangThai);

    }
}
