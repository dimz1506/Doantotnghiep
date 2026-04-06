using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.IRepository;
using Doantotnghiep.Services.Interfaces;

namespace Doantotnghiep.Services.Implementations
{
    public class LoaiDichVuServices : ILoaiDichVuServices
    {
        private readonly ILoaiDichVuRepository _loaidichvuRepository;
        public LoaiDichVuServices(ILoaiDichVuRepository loaidichvuRepository)
        {
            _loaidichvuRepository = loaidichvuRepository;
        }
        public async Task<(bool ok, string error)> CreateLoaiDichVuAsync(LoaiDichVu loaiDichVu)
        {
            var loaidichvu1 = await _loaidichvuRepository.CreateLoaiDichVuAsync(loaiDichVu);
            return loaidichvu1;
        }
        public async Task<(bool ok, string error)> DeleteLoaiDichVuAsync(int id)
        {
            var loaidichvu1 = await _loaidichvuRepository.DeleteLoaiDichVuAsync(id);
            if (!loaidichvu1.ok)
            {
                return (false, loaidichvu1.error);
            }
            return loaidichvu1;
        }
        public async Task<List<LoaiDichVu>> GetAllLoaiDichVuAsync(string? searchString)
        {
            return await _loaidichvuRepository.GetAllLoaiDichVuAsync(searchString);
        }
        public async Task<LoaiDichVu> GetLoaiDichVuByIdAsync(int id)
        {
            return await _loaidichvuRepository.GetLoaiDichVuByIdAsync(id);
        }
        public async Task<(bool ok, string error)> UpdateLoaiDichVuAsync(LoaiDichVu loaiDichVu)
        {
            var loaidichvu1 = await _loaidichvuRepository.UpdateLoaiDichVuAsync(loaiDichVu);
            if (!loaidichvu1.ok)
            {
                return (false, loaidichvu1.error);
            }
            return loaidichvu1;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _loaidichvuRepository.SaveChangesAsync();
        }
    }
}
