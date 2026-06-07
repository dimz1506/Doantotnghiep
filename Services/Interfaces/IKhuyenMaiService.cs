using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IKhuyenMaiService
    {
        Task<List<KhuyenMai>> GetAllAsync();
        Task<KhuyenMai> GetByIdAsync(int id);
        Task<KhuyenMaiViewModel> GetCreateViewModelAsync();
        Task<KhuyenMaiViewModel> GetEditViewModelAsync(int id);
        Task CreateAsync(KhuyenMaiViewModel model);
        Task UpdateAsync(KhuyenMaiViewModel model);
        Task DeleteAsync(int id);
        Task<List<SelectListItem>> GetDichVuSelectListAsync();
    }
}
