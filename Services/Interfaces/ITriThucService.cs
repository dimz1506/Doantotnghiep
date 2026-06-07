using Doantotnghiep.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doantotnghiep.Services.Interfaces
{
    public interface ITriThucService
    {
        Task<List<TriThuc>> GetAllAsync();
        Task<TriThuc> GetByIdAsync(int id);
        Task AddAsync(TriThuc triThuc);
        Task UpdateAsync(TriThuc triThuc);
        Task DeletedAsync(int id);
        Task<List<SelectListItem>> GetLoaiTriThucSelectListAsync();
    }
}
