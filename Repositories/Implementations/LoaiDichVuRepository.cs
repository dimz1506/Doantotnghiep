using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Repository
{
    public class LoaiDichVuRepository : ILoaiDichVuRepository
    {
        private readonly AppDbContext _context;
        public LoaiDichVuRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool ok, string error)> CreateLoaiDichVuAsync(LoaiDichVu loaiDichVu)
        {
            try
            {
                await _context.LoaiDichVus.AddAsync(loaiDichVu);
                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool ok, string error)> DeleteLoaiDichVuAsync(int id)
        {
            try
            {
                var loaiDichVu = await _context.LoaiDichVus.FindAsync(id);
                if (loaiDichVu == null)
                {
                    return (false, "Loại dịch vụ không tồn tại.");
                }
                _context.LoaiDichVus.Remove(loaiDichVu);
                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<List<LoaiDichVu>> GetAllLoaiDichVuAsync(string? searchString)
        {
            try
            {
                var query = _context.LoaiDichVus.AsQueryable();
                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(ldv => ldv.TenLoaiDichVu.Contains(searchString));
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return (new List<LoaiDichVu>());
            }
        }

        public async Task<LoaiDichVu> GetLoaiDichVuByIdAsync(int id)
        {
            try
            {
                return await _context.LoaiDichVus.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<(bool ok, string error)> UpdateLoaiDichVuAsync(LoaiDichVu loaiDichVu)
        {
            try
            {
                var TenLoaidichvu = await _context.LoaiDichVus
                    .Where(ldv => ldv.IdLoaiDichVu != loaiDichVu.IdLoaiDichVu && ldv.TenLoaiDichVu == loaiDichVu.TenLoaiDichVu)
                    .FirstOrDefaultAsync();
                if (TenLoaidichvu != null)
                {
                    return (false, "Tên loại dịch vụ đã tồn tại.");
                }
                _context.LoaiDichVus.Update(loaiDichVu);
                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
