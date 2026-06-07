using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
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

        public async Task<NhanVien?> GetNhanVienByIdAsync(int id)
        {
            //kiem tra id nhan vien co ton tai khong
            var nhanvien = await _nhanVienRepository.GetNhanVienByIdAsync(id);
            if (nhanvien == null)
            {
                return null;
            }
            return nhanvien;
        }

        public async Task<NhanVien?> GetNhanVienByTaiKhoanIdAsync(int taiKhoanId)
        {
            var nhanvien = await _nhanVienRepository.GetNhanVienByTaiKhoanIdAsync(taiKhoanId);
            if (nhanvien == null)
            {
                return null;
            }
            return nhanvien;
        }

        public async Task<List<NhanVien>> GetNhanVienByTrangThaiAsync(TrangThaiNhanVien trangThai)
        {
            //kiem tra null
            var nhanvien = await _nhanVienRepository.GetNhanVienByTrangThaiAsync(trangThai);
            if (nhanvien == null)
            {
                return new List<NhanVien>();
            }
            return nhanvien;
        }

     
        public async Task<(bool ok, string error)> UpdateNhanVienAsync(NhanVien nhanVien)
        {
             if (nhanVien.IdNhanVien <= 0)
            {
                return (false, "ID nhân viên không hợp lệ.");
            }

            if (string.IsNullOrWhiteSpace(nhanVien.TenNhanVien))
            {
                return (false, "Tên nhân viên không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(nhanVien.ChuyenMonNV))
            {
                return (false, "Chuyên môn không được để trống.");
            }

            var nhanVienDb = await _nhanVienRepository.GetNhanVienByIdAsync(nhanVien.IdNhanVien);
            if (nhanVienDb == null)
            {
                return (false, "Không tìm thấy nhân viên.");
            }

            nhanVienDb.TenNhanVien = nhanVien.TenNhanVien;
            nhanVienDb.DiaChiNV = nhanVien.DiaChiNV;
            nhanVienDb.ChuyenMonNV = nhanVien.ChuyenMonNV;
            nhanVienDb.TrangThaiNV = nhanVien.TrangThaiNV;
            nhanVienDb.LaNhanVienFullTime = nhanVien.LaNhanVienFullTime;
            nhanVienDb.GioBatDauLamViec = nhanVien.GioBatDauLamViec;
            nhanVienDb.GioKetThucLamViec = nhanVien.GioKetThucLamViec;

            return await _nhanVienRepository.UpdateNhanVienAsync(nhanVienDb);
        }
    }
}
