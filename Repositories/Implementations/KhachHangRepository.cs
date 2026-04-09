using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly AppDbContext _context;
        public KhachHangRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<KhachHang>> GetAllKhachHangAsync()
        {
            return await _context.KhachHangs.Include(kh => kh.TaiKhoan).ToListAsync();

        }

        public async Task<KhachHang?> GetKhachHangByIdAsync(int id)
        {
            return await _context.KhachHangs.Include(kh => kh.TaiKhoan).FirstOrDefaultAsync(kh => kh.IdKhachHang == id);
        }

        public async Task<KhachHang?> GetKhachHangByTaiKhoanIdAsync(int taiKhoanId)
        {
            return await _context.KhachHangs.Include(kh => kh.TaiKhoan).FirstOrDefaultAsync(kh => kh.IdTaiKhoan == taiKhoanId);
        }

        public async Task<List<KhachHang>> GetKhachHangByTenAsync(string? tenKH)
        {
            return await _context.KhachHangs.Include(kh => kh.TaiKhoan).Where(kh => kh.TenKhachHang.Contains(tenKH)).ToListAsync();
        }

        public async Task<(bool ok, string error)> UpdateKhachHangAsync(KhachHang khachHang)
        {
                try
                {
                    _context.KhachHangs.Update(khachHang);
                    var result =await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        return (true, "Cập nhật khách hàng thành công!");
                    }
                    return (false, "Cập nhật khách hàng thất bại");
                }
                catch (Exception ex)
                {
                    return (false, $"Lỗi khi cập nhật khách hàng: {ex.Message}");
                }
        }
    }
}
