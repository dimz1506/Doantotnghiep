using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;

namespace Doantotnghiep.Services.Implementations
{
    public class NhanVienServices : INhanVienServices
    {
        private readonly INhanVienRepository _nhanVienRepository;
        public NhanVienServices(INhanVienRepository nhanVienRepository)
        {
            _nhanVienRepository = nhanVienRepository;
        }

        public async Task<(bool ok, string error)> CreateNhanVienAsync(NhanVien nhanVien)
        {
            //kiem tra null
            if (string.IsNullOrEmpty(nhanVien.TenNhanVien))
            {
                return (false, " Ten nhan vien khong duoc de trong.");
            }
            //
            var nhanvien1 = await _nhanVienRepository.CreateNhanVienAsync(nhanVien);
            return nhanvien1;
        }

        public async Task<(bool ok, string error)> DeleteNhanVienAsync(int id)
        {
            //kiem tra id nhan vien co ton tai khong
            var nhanvien = await _nhanVienRepository.GetNhanVienByIdAsync(id);
            if (nhanvien == null)
            {
                return (false, "Khong tim thay nhan vien.");
            }
            var nhanvien1 = await _nhanVienRepository.DeleteNhanVienAsync(id);
            if (!nhanvien1.ok)
            {
                return (false, "Xoa khong thanh cong.");
            }
            return (true, "Xoa nhan vien thanh cong.");
        }

        public async Task<List<NhanVien>> GetAllNhanVienAsync()
        {
            var nhanvien1 = await _nhanVienRepository.GetAllNhanVienAsync();
            return nhanvien1;
        }

        public async Task<List<NhanVien>> GetNhanVienByChuyenMonAsync(string? chuyenMon)
        {
            // kiem tra chuyen mon khong duoc de trong
            var chuyenmon = await _nhanVienRepository.GetNhanVienByChuyenMonAsync(chuyenMon);
            if (chuyenmon == null || chuyenmon.Count == 0)
            {
                return new List<NhanVien>();
            }
            return chuyenmon;
        }

        public async Task<NhanVien> GetNhanVienByIdAsync(int id)
        {
            //kiem tra id nhan vien co ton tai khong
            var nhanvien = await _nhanVienRepository.GetNhanVienByIdAsync(id);
            if (nhanvien == null)
            {
                return null;
            }
            return nhanvien;
        }

        public async Task<List<NhanVien>> GetNhanVienByTrangThaiAsync(bool trangThai)
        {
            //kiem tra null
            var trangthai = await _nhanVienRepository.GetNhanVienByTrangThaiAsync(trangThai);
            if (trangthai == null || trangthai.Count == 0)
            {
                return new List<NhanVien>();
            }
            return trangthai;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _nhanVienRepository.SaveChangeAsync();
        }

        public async Task<(bool ok, string error)> UpdateNhanVienAsync(NhanVien nhanVien)
        {
            //kiem tra id nhan vien co ton tai khong
            var nhanvien = await _nhanVienRepository.GetNhanVienByIdAsync(nhanVien.IdNhanVien);
            if (nhanvien == null)
            {
                return (false, "Khong tim thay nhan vien.");
            }
            if(string.IsNullOrEmpty(nhanVien.TenNhanVien))
            {
                return (false, " Ten nhan vien khong duoc de trong.");
            }
            var nhanvien1 = await _nhanVienRepository.UpdateNhanVienAsync(nhanVien);
            if (!nhanvien1.ok)
            {
                return (false, "Cap nhat khong thanh cong.");
            }
            return (true, "Cap nhat nhan vien thanh cong.");
        }
    }
}
