using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class NhanVienRepository : INhanVienRepository
    {
        private readonly AppDbContext _context;
        public NhanVienRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<NhanVien>> GetAllNhanVienAsync()
        {
            return await _context.NhanViens.Include(nv => nv.TaiKhoan).ToListAsync();
        }

        public async Task<List<NhanVien>> GetNhanVienByChuyenMonAsync(string? chuyenMon)
        {
            if(string.IsNullOrEmpty(chuyenMon))
            {
                return new List<NhanVien>();
            }
            return await _context.NhanViens.Include(nv => nv.TaiKhoan).Where(nv => nv.ChuyenMonNV.Contains(chuyenMon)).ToListAsync();

        }

        public async Task<NhanVien?> GetNhanVienByIdAsync(int id)
        {
            return await _context.NhanViens
                .Include(nv => nv.TaiKhoan)
                .Include(nv => nv.NhanVienDichVus)
                    .ThenInclude(nvdv => nvdv.DichVu)
                .FirstOrDefaultAsync(nv => nv.IdNhanVien == id);
        }

        public async Task<NhanVien?> GetNhanVienByTaiKhoanIdAsync(int taiKhoanId)
        {
            return await _context.NhanViens.Include(nv => nv.TaiKhoan).FirstOrDefaultAsync(nv => nv.IdTaiKhoan == taiKhoanId);

        }

        public async Task<List<NhanVien>> GetNhanVienByTrangThaiAsync(TrangThaiNhanVien trangThai)
        {
            return await _context.NhanViens.Include(nv => nv.TaiKhoan).Where(nv => nv.TrangThaiNV == trangThai).ToListAsync();
        }

        
        public async Task<(bool ok, string error)> UpdateNhanVienAsync(NhanVien nhanVien)
        {
            _context.NhanViens.Update(nhanVien);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return (true, "Cap nhat nhan vien thanh cong!");
            }
            return (false, "Cập nhật nhân viên thất bại");
        }
    }
}
