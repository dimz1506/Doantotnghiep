using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;

namespace Doantotnghiep.Services.Implementations
{
    public class KhachHangServices : IKhachHangServices
    {
        private readonly IKhachHangRepository _khachHangRepository;
        public KhachHangServices(IKhachHangRepository khachHangRepository)
        {
            _khachHangRepository = khachHangRepository;
        }

        public async Task<List<KhachHang>> GetAllKhachHangAsync()
        {
            var khachhang1 = await _khachHangRepository.GetAllKhachHangAsync();
            return khachhang1;
        }

        public async Task<KhachHang?> GetKhachHangByIdAsync(int id)
        {
            var khachhang = await _khachHangRepository.GetKhachHangByIdAsync(id);
            if (khachhang == null)
            {
                return null;
            }
            return khachhang;
        }

        public async Task<KhachHang?> GetKhachHangByTaiKhoanIdAsync(int taiKhoanId)
        {
            var khachhang = await _khachHangRepository.GetKhachHangByTaiKhoanIdAsync(taiKhoanId);
            if (khachhang == null)
            {
                return null;
            }
            return khachhang;
        }

        public async Task<List<KhachHang>> GetKhachHangByTenAsync(string? tenKH)
        {
            if(string.IsNullOrWhiteSpace(tenKH))
            {
                return new List<KhachHang>();
            }
            var khachhang = await _khachHangRepository.GetKhachHangByTenAsync(tenKH);
            if (khachhang.Count == 0)
            {
                return new List<KhachHang>();
            }
            return khachhang;
        }

        public async Task<(bool ok, string error)> UpdateKhachHangAsync(KhachHang khachHang)
        {
            var existingKhachHang = await _khachHangRepository.GetKhachHangByIdAsync(khachHang.IdKhachHang);
            if (existingKhachHang == null)
            {
                return (false, "Khách hàng không tồn tại");
            }
            if(string.IsNullOrWhiteSpace(khachHang.TenKhachHang))
            {
                return (false, "Tên khách hàng không được để trống");
            }
            existingKhachHang.TenKhachHang = khachHang.TenKhachHang;
            existingKhachHang.DiaChiKhachHang = khachHang.DiaChiKhachHang;
            existingKhachHang.GhiChuKH = khachHang.GhiChuKH;

            var result = await _khachHangRepository.UpdateKhachHangAsync(existingKhachHang);
            if (!result.ok)
            {
                return (false, result.error);
            }
            return (true, "Cập nhật khách hàng thành công!");

        }
    }
}
