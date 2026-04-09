using Doantotnghiep.Models.Entities;

namespace Doantotnghiep.Repositories.Interface
{
    public interface IKhachHangRepository
    {
        Task<List<KhachHang>> GetAllKhachHangAsync();
        Task<KhachHang?> GetKhachHangByIdAsync(int id);
        Task<(bool ok, string error)> UpdateKhachHangAsync(KhachHang khachHang);
        Task<KhachHang?> GetKhachHangByTaiKhoanIdAsync(int taiKhoanId);
        Task<List<KhachHang>> GetKhachHangByTenAsync(string? tenKH);
    }
}
