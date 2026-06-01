using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;

namespace Doantotnghiep.Services.Implementations
{
    public class LoaiTriThucService : ILoaiTriThucService
    {
        private readonly ILoaiTriThucRepository _loaiTriThucRepository;
        public LoaiTriThucService (ILoaiTriThucRepository loaiTriThucRepository)
        {
            _loaiTriThucRepository = loaiTriThucRepository;
        }

        public async Task AddAsync(LoaiTriThuc loaiTriThuc)
        {
           loaiTriThuc.NgayTaoLoaiTriThuc = DateTime.Now;
            await _loaiTriThucRepository.AddAsync(loaiTriThuc);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await GetByIdAsync(id);
            data.IsDeleted = true;
            data.NgayXoaLoaiTriThuc = DateTime.Now;

            await _loaiTriThucRepository.UpdateAsync(data);
        }

        public async Task<List<LoaiTriThuc>> GetAllAsync()
        {
            return await _loaiTriThucRepository.GetAllAsync();
        }

        public async Task<LoaiTriThuc> GetByIdAsync(int id)
        {
            var data = await _loaiTriThucRepository.GetByIdAsync(id);
            if (data == null)
            {
                throw new Exception("Khong tim thay loai tri thuc.");
            }
            return data;
        }

        public async Task UpdateAsync(LoaiTriThuc loaiTriThuc)
        {
            var data = await GetByIdAsync(loaiTriThuc.IdLoaiTriThuc);

            data.TenLoaiTriThuc = loaiTriThuc.TenLoaiTriThuc;
            data.MoTaLoaiTriThuc = loaiTriThuc.MoTaLoaiTriThuc;
            data.NgayCapNhatLoaiTriThuc = DateTime.Now;

            await _loaiTriThucRepository.UpdateAsync(data);
        }
    }
}
