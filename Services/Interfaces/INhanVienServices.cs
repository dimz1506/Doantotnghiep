using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;

namespace Doantotnghiep.Services.Interfaces
{
    public interface INhanVienServices
    {
        Task<List<NhanVien>> GetAllNhanVienAsync();
        Task<NhanVien?> GetNhanVienByIdAsync(int id);
        Task<(bool ok, string error)> UpdateNhanVienAsync(NhanVien nhanVien);
        Task<List<NhanVien>> GetNhanVienByChuyenMonAsync(string? chuyenMon);
        Task<List<NhanVien>> GetNhanVienByTrangThaiAsync(TrangThaiNhanVien trangThai);
        Task<NhanVien?> GetNhanVienByTaiKhoanIdAsync(int taiKhoanId);
        
    }
}
