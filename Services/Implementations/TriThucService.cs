using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Services.Implementations
{
    public class TriThucService : ITriThucService
    {
        private readonly ITriThucRepository _repository;
        private readonly AppDbContext _context;
        public TriThucService(ITriThucRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task AddAsync(TriThuc triThuc)
        {
            triThuc.NgayNhapTriThuc = DateTime.Now;
            triThuc.TrangThaiTriThuc = true;
            triThuc.IsDeleted = false;
            
            await _repository.AddAsync(triThuc);
        }

        public async Task DeletedAsync(int id)
        {
            var data = await GetByIdAsync(id);

            data.IsDeleted = true;
            data.NgayXoaTriThuc = DateTime.Now;
            data.TrangThaiTriThuc = false;

            await _repository.UpdateAsync(data); 
        }

        public async Task<List<TriThuc>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TriThuc> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            if(data == null)
            {
                throw new Exception("Khong tim thay tri thuc.");
            }
            return data;
        }

        public async Task<List<SelectListItem>> GetLoaiTriThucSelectListAsync()
        {
            return await _context.LoaiTriThucs
               .Where(x => !x.IsDeleted)
               .OrderBy(x => x.TenLoaiTriThuc)
               .Select(x => new SelectListItem
               {
                   Value = x.IdLoaiTriThuc.ToString(),
                   Text = x.TenLoaiTriThuc
               })
               .ToListAsync();
        }

        public async Task UpdateAsync(TriThuc triThuc)
        {
            var data = await GetByIdAsync(triThuc.IdTriThuc);

            data.TenTriThuc = triThuc.TenTriThuc;
            data.NoiDungTriThuc = triThuc.NoiDungTriThuc;
            data.TuKhoa = triThuc.TuKhoa;
            data.IdLoaiTriThuc = triThuc.IdLoaiTriThuc;
            data.TrangThaiTriThuc = triThuc.TrangThaiTriThuc;
            data.NgayCapNhatTriThuc = DateTime.Now;

            await _repository.UpdateAsync(data);

        }
    }
}
