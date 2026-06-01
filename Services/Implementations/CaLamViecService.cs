using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;

namespace Doantotnghiep.Services.Implementations
{
    public class CaLamViecService : ICaLamViecService
    {
        private readonly ICaLamViecRepository _caLamViecRepository;
        public CaLamViecService(ICaLamViecRepository caLamViecRepository)
        {
            _caLamViecRepository = caLamViecRepository;
        }

        public async Task AddAsync(CaLamViec caLamViec)
        {
            await _caLamViecRepository.AddAsync(caLamViec);
        }

        public async Task<List<CaLamViec>> GetAllAsync()
        {
            return await _caLamViecRepository.GetAllAsync();
        }

        public async Task<CaLamViec> GetByIdAsync(int id)
        {
            var data = await _caLamViecRepository.GetByIdAsync(id);
            if (data == null)
            {
                throw new Exception("Khong tim thay ca lam viec.");
            }
            return data;
        }

        public async Task<List<CaLamViec>> GetCaDangMoAsync()
        {
            return await _caLamViecRepository.GetCaDangMoAsync();
        }

        public async Task UpdateAsync(CaLamViec caLamViec)
        {
            await _caLamViecRepository.UpdateAsync(caLamViec);
        }
    }
}
