using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Repositories.Implementations
{
    public class DichVuRepository : IDichVuRepository
    {
        private readonly Data.AppDbContext _context;
        public DichVuRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool ok, string error)> CreateDichVuAsync(DichVu dichVu)
        {

            try
            {
                _context.DichVus.Add(dichVu);
                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool ok, string error)> DeleteDichVuAsync(int id)
        {
            try
            {
                _context.DichVus.Remove(_context.DichVus.Find(id));
                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<List<DichVu>> GetAllDichVuAsync(string? searchString)
        {
            try
            {
                var query = _context.DichVus.Include(dv => dv.LoaiDichVu).AsQueryable();
                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(dv => dv.TenDichVu.Contains(searchString));
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<DichVu>();
            }
        }

        public async Task<DichVu> GetDichVuByIdAsync(int id)
        {
            try
            {
                return await _context.DichVus.Include(dv => dv.LoaiDichVu).FirstOrDefaultAsync(dv => dv.IdDichVu == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<(bool ok, string error)> UpdateDichVuAsync(DichVu dichVu)
        {
            try
            {
                _context.DichVus.Update(dichVu);
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
