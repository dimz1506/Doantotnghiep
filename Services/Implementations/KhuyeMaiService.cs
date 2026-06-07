using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Repositories.Implementations;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Services.Implementations
{
    public class KhuyeMaiService : IKhuyenMaiService
    {
        public readonly IKhuyenMaiRepository _KhuyenMaiRepository;
        public readonly AppDbContext _context;
        public KhuyeMaiService(IKhuyenMaiRepository khuyenMaiRepository, AppDbContext context)
        {
            _context = context;
            _KhuyenMaiRepository = khuyenMaiRepository;
        }

        public async Task CreateAsync(KhuyenMaiViewModel model)
        {
            ValidateKhuyenMai(model);

            var khuyenMai = new KhuyenMai
            {
                TenKhuyenMai = model.TenKhuyenMai,
                MoTaKhuyenMai = model.MoTaKhuyenMai,
                NgayBatDau = model.NgayBatDau,
                NgayKetThuc = model.NgayKetThuc,
                LoaiKhuyenMai = model.LoaiKhuyenMai,
                GiaTriGiam = model.GiaTriGiam,
                TrangThaiKhuyenMai = model.TrangThaiKhuyenMai,
                NgayTaoKhuyenMai = DateTime.Now,
                IsDeleted = false,
                DichVuKhuyenMais = model.IdDichVus.Select(idDichVu => new DichVuKhuyenMai
                {
                    IdDichVu = idDichVu
                }).ToList()
            };

            await _KhuyenMaiRepository.AddAsync(khuyenMai);
        }

        public async Task DeleteAsync(int id)
        {
            var khuyenMai = await GetByIdAsync(id);
            await _KhuyenMaiRepository.SoftDeleteAsync(khuyenMai);
        }

        public async Task<List<KhuyenMai>> GetAllAsync()
        {
            return await _KhuyenMaiRepository.GetAllAsync();
        }

        public async Task<KhuyenMai> GetByIdAsync(int id)
        {
            var khuyenMai = await _KhuyenMaiRepository.GetByIdAsync(id);
            if (khuyenMai == null)
            {
                throw new Exception("Khong tim thay khuyen mai.");
            }
            return khuyenMai;
        }

        public async Task<KhuyenMaiViewModel> GetCreateViewModelAsync()
        {
            return new KhuyenMaiViewModel
            {
                NgayBatDau = DateTime.Today,
                NgayKetThuc = DateTime.Today.AddDays(7),
                TrangThaiKhuyenMai = true,
                DichVus = await GetDichVuSelectListAsync()
            };

        }


        public async Task<List<SelectListItem>> GetDichVuSelectListAsync()
        {
            return await _context.DichVus
                .Where(x => x.TrangThaiDV == true)
                .OrderBy(x => x.TenDichVu)
                .Select(x => new SelectListItem
                {
                    Value = x.IdDichVu.ToString(),
                    Text = x.TenDichVu
                })
                .ToListAsync();
        }

        public async Task<KhuyenMaiViewModel> GetEditViewModelAsync(int id)
        {
            var khuyenMai = await GetByIdAsync(id);

            return new KhuyenMaiViewModel
            {
                IdKhuyenMai = khuyenMai.IdKhuyenMai,
                TenKhuyenMai = khuyenMai.TenKhuyenMai,
                MoTaKhuyenMai = khuyenMai.MoTaKhuyenMai,
                NgayBatDau = khuyenMai.NgayBatDau,
                NgayKetThuc = khuyenMai.NgayKetThuc,
                LoaiKhuyenMai = khuyenMai.LoaiKhuyenMai,
                GiaTriGiam = khuyenMai.GiaTriGiam,
                TrangThaiKhuyenMai = khuyenMai.TrangThaiKhuyenMai,
                IdDichVus = khuyenMai.DichVuKhuyenMais
                    .Select(x => x.IdDichVu)
                    .ToList(),
                DichVus = await GetDichVuSelectListAsync()
            };
        }

        public async Task UpdateAsync(KhuyenMaiViewModel model)
        {
            ValidateKhuyenMai(model);

            var khuyenMai = await _context.KhuyenMais
                .Include(x => x.DichVuKhuyenMais)
                .FirstOrDefaultAsync(x => x.IdKhuyenMai == model.IdKhuyenMai && !x.IsDeleted);

            if (khuyenMai == null)
            {
                throw new Exception("Không tìm thấy khuyến mãi.");
            }

            khuyenMai.TenKhuyenMai = model.TenKhuyenMai;
            khuyenMai.MoTaKhuyenMai = model.MoTaKhuyenMai;
            khuyenMai.NgayBatDau = model.NgayBatDau;
            khuyenMai.NgayKetThuc = model.NgayKetThuc;
            khuyenMai.LoaiKhuyenMai = model.LoaiKhuyenMai;
            khuyenMai.GiaTriGiam = model.GiaTriGiam;
            khuyenMai.TrangThaiKhuyenMai = model.TrangThaiKhuyenMai;
            khuyenMai.NgayCapNhatKhuyenMai = DateTime.Now;

            _context.DichVuKhuyenMais.RemoveRange(khuyenMai.DichVuKhuyenMais);

            khuyenMai.DichVuKhuyenMais = model.IdDichVus.Select(idDichVu => new DichVuKhuyenMai
            {
                IdDichVu = idDichVu,
                IdKhuyenMai = khuyenMai.IdKhuyenMai
            }).ToList();

            await _KhuyenMaiRepository.UpdateAsync(khuyenMai);
        }

        private void ValidateKhuyenMai(KhuyenMaiViewModel model)
        {
            if (model.NgayKetThuc < model.NgayBatDau)
            {
                throw new Exception("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");
            }

            if (model.LoaiKhuyenMai == Models.Enum.LoaiKhuyenMai.GiamTheoPhanTram)
            {
                if (model.GiaTriGiam <= 0 || model.GiaTriGiam > 100)
                {
                    throw new Exception("Giá trị giảm theo phần trăm phải nằm trong khoảng 1 đến 100.");
                }
            }

            if (model.LoaiKhuyenMai == Models.Enum.LoaiKhuyenMai.GiamTheoSoTien)
            {
                if (model.GiaTriGiam <= 0)
                {
                    throw new Exception("Giá trị giảm theo số tiền phải lớn hơn 0.");
                }
            }
        }
    }
}
