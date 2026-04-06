using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
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

        public async Task<(bool ok, string error)> CreateNhanVienAsync(NhanVien nhanVien)
        {
            _context.NhanViens.Add(nhanVien);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return (true, "Them nhan vien thanh cong!");
            }
            return (false, "Thêm nhân viên thất bại");
        }

        public async Task<(bool ok, string error)> DeleteNhanVienAsync(int id)
        {
            var nhanvien = await _context.NhanViens.FindAsync(id);
            if (nhanvien == null)
            {
                return (false, "Khong tim thay nhan vien.");
            }
            _context.NhanViens.Remove(nhanvien);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return (true, "Xoa nhan vien thanh cong!");
            }
            return (false, "Xóa nhân viên thất bại");
        }

        public async Task<List<NhanVien>> GetAllNhanVienAsync()
        {
            return await _context.NhanViens.ToListAsync();
        }

        public async Task<List<NhanVien>> GetNhanVienByChuyenMonAsync(string? chuyenMon)
        {
            return await _context.NhanViens.Where(nv => nv.ChuyenMonNV.Contains(chuyenMon)).ToListAsync();

        }

        public async Task<NhanVien> GetNhanVienByIdAsync(int id)
        {
            return await _context.NhanViens.FindAsync(id);
        }

        public async Task<List<NhanVien>> GetNhanVienByTrangThaiAsync(bool trangThai)
        {
            return await _context.NhanViens.Where(nv => nv.TrangThaiNV == (trangThai ? Models.Enum.TrangThaiNhanVien.DangLamViec : Models.Enum.TrangThaiNhanVien.NghiViec)).ToListAsync();
        }

        public async Task<int> SaveChangeAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
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
