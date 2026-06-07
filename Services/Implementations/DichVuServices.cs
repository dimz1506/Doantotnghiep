using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Doantotnghiep.Models.ViewModel;

namespace Doantotnghiep.Services.Implementations
{
    public class DichVuServices : IDichVuServices
    {
        private readonly AppDbContext _context;
        private readonly IDichVuRepository _dichvuRepository;
        public DichVuServices(IDichVuRepository dichvuRepository, AppDbContext appDbContext)
        {
            _dichvuRepository = dichvuRepository;
            _context = appDbContext;
        }
        public async Task<(bool ok, string error)> CreateDichVuAsync(DichVu dichVu)
        {
            //kiem tra ten dich vu khong duoc de trong
            if (string.IsNullOrEmpty(dichVu.TenDichVu))
            {
                return (false, "Ten dich vu khong duoc de trong.");
            }
            // kiem tra ten dich vu da ton tai chua
            var allDichVu = await _dichvuRepository.GetAllDichVuAsync(null, null);
            var exists = allDichVu.Any(dv => dv.TenDichVu.ToLower() == dichVu.TenDichVu.ToLower());
            if(exists)
            {
                return (false, "Tên dịch vụ đã tồn tại");
            }
            var dichvu1 = await _dichvuRepository.CreateDichVuAsync(dichVu);
            return dichvu1;
        }

        public async Task<(bool ok, string error)> DeleteDichVuAsync(int id)
        {
            var dichvu = await _dichvuRepository.GetDichVuByIdAsync(id);
            if (dichvu == null)
            {
                return (false, "Dịch vụ không tồn tại.");
            }
            var dichvu1 = await _dichvuRepository.DeleteDichVuAsync(id);
            if (!dichvu1.ok)
            {
                return (false, dichvu1.error);
            }
            return dichvu1;
        }

        public async Task<List<DichVu>> GetAllDichVuAsync(string? searchString, int? idLoaiDichVu)
        {
            return await _dichvuRepository.GetAllDichVuAsync(searchString, idLoaiDichVu);
        }

        public async Task<DichVu> GetDichVuByIdAsync(int id)
        {
            var dichvu1 = await _dichvuRepository.GetDichVuByIdAsync(id);
            return dichvu1;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dichvuRepository.SaveChangesAsync();
        }

        public async Task<(bool ok, string error)> UpdateDichVuAsync(DichVu dichVu)
        {
            //kiem tra rong
            if(string.IsNullOrEmpty(dichVu.TenDichVu))
            {
                return (false, "Ten dich vu khong duoc de trong.");
            }
            //kiem tra trung ten dich vu
            var allDichVu = await _dichvuRepository.GetAllDichVuAsync(null,null);
            var exists = allDichVu.Any(dv => dv.TenDichVu.ToLower() == dichVu.TenDichVu.ToLower() && dv.IdDichVu != dichVu.IdDichVu);
            if (exists)
            {
                return (false, "Tên dịch vụ đã tồn tại");
            }
            var dichvu1 = await _dichvuRepository.UpdateDichVuAsync(dichVu);
            if (!dichvu1.ok)
            {
                return (false, dichvu1.error);
            }
            return dichvu1;
        }
        public async Task<List<DichVu>> GetDichVusByIdAsync(List<int> id)
        {
            return await _dichvuRepository.GetDichVusByIdAsync(id);
        }
        public async Task<List<DichVuKhachDatNhieuVM>> GetDichVuKhachDatNhieuAsync(int idKhachHang)
        {
            return await _context.ChiTietDatLiches
                .Include(x => x.DatLich)
                .Include(x => x.DichVu)
                .Where(x =>
                    x.DatLich != null &&
                    x.DatLich.IdKhachHang == idKhachHang &&
                    x.DichVu != null &&
                    !x.DichVu.IsDeleted &&
                    x.DichVu.TrangThaiDV)
                .GroupBy(x => new
                {
                    x.IdDichVu,
                    x.DichVu.TenDichVu,
                    x.DichVu.HinhAnhDichVu,
                    x.DichVu.GiaDichVu,
                    x.DichVu.ThoiLuongDV
                })
                .Select(g => new DichVuKhachDatNhieuVM
                {
                    IdDichVu = g.Key.IdDichVu,
                    TenDichVu = g.Key.TenDichVu,
                    HinhAnhDichVu = g.Key.HinhAnhDichVu,
                    GiaDichVu = g.Key.GiaDichVu,
                    ThoiLuongDV = g.Key.ThoiLuongDV,
                    SoLanDat = g.Count()
                })
                .OrderByDescending(x => x.SoLanDat)
                .Take(5)
                .ToListAsync();
        }
    }
}
