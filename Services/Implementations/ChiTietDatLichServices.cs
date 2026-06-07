using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;

namespace Doantotnghiep.Services.Implementations
{
    public class ChiTietDatLichServices : IChiTietDatLichServices
    {
        private readonly IChiTietDatLichRepository _chiTietDatLichRepository;
        public ChiTietDatLichServices(IChiTietDatLichRepository chiTietDatLichRepository)
        {
            _chiTietDatLichRepository = chiTietDatLichRepository;
        }

        public async Task<bool> CreateAsync(ChiTietDatLich chiTietDatLich)
        {
            if(chiTietDatLich == null)
            {
                return false;
            }
            await _chiTietDatLichRepository.AddAsync(chiTietDatLich);
            await _chiTietDatLichRepository.SaveChangeAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _chiTietDatLichRepository.GetByIdAsync(id);
            if(entity == null)
            {
                return false;
            }
            await _chiTietDatLichRepository.DeleteAsync(entity);
            await _chiTietDatLichRepository.SaveChangeAsync();
            return true;
        }

        public async Task<List<ChiTietDatLich>> GetAllAsync()
        {
            return await _chiTietDatLichRepository.GetAllAsync();
        }

        public async Task<List<ChiTietDatLich>> GetByDatLichIdAsync(int idDatLich)
        {
            return await _chiTietDatLichRepository.GetByDatLichIdAsync(idDatLich);
        }

        public async Task<ChiTietDatLich?> GetByIdAsync(int id)
        {
            return await _chiTietDatLichRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(ChiTietDatLich chiTietDatLich)
        {
            if(chiTietDatLich != null)
            {
                return false;
            }
            await _chiTietDatLichRepository.UpdateAsync(chiTietDatLich);
            await _chiTietDatLichRepository.SaveChangeAsync();
            return true;
        }
    }
}
