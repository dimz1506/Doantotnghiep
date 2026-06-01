using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;

namespace Doantotnghiep.Services.Implementations
{
    public class LichLamViecServices : ILichLamViecServices
    {
        private readonly ILichlamViecRepository _lichlamViecRepository;
        public LichLamViecServices (ILichlamViecRepository lichlamViecRepository)
        {
            _lichlamViecRepository = lichlamViecRepository;
        }

        public async Task AddAsync(LichLamViecNhanVien lichLamViecNV)
        {
            await _lichlamViecRepository.AddAsync(lichLamViecNV);
        }

        public async Task<List<LichLamViecNhanVien>> GetAllAsync()
        {
            var data = await _lichlamViecRepository.GetAllAsync();
            return data;
        }

        public async Task<LichLamViecNhanVien> GetByIdAsync(int id)
        {
            var data = await _lichlamViecRepository.GetByIdAsync(id);
            if(data == null)
            {
                throw new Exception("Khong tim thay lich lam viec.");
            }
            return data;
        }

        public async Task<List<LichLamViecNhanVien>> GetByNhanVienAsync(int idNhanVien)
        {
            var data = await _lichlamViecRepository.GetByNhanVienAsync(idNhanVien);
            return data;
        }
    }
}
