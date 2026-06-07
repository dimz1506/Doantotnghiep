using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
using Doantotnghiep.Models.ViewModel;

namespace Doantotnghiep.Repositories.Interface
{
    public interface IDatLichRepository
    {
        Task<List<DatLich>> GetAllDatLichAsync();
        Task<DatLich?> GetDatLichByIdAsync(int id);
        Task<List<DatLich>> GetDatLichByKhachHangIdAsync(int khachHangId);
        Task<List<DatLich>> GetDatLichByNhanVienIdAsync(int nhanVienId);
        Task<List<DatLich>> GetDatLichByTrangThaiAsync(TrangThaiDatLich trangThai);
        Task<List<DatLich>> GetDatLichByNgayHenLichAsync(DateTime ngayHenLich);
        Task<bool> KiemTraTrungLichAsync(int IdNhanVien, DateTime ngayHenLich, DateTime gioBatDauDV, DateTime gioKetThucDV, int? IdDatLich);
        Task<DatLich> CreateDatLichAsync(DatLich datLich);
        Task<bool> UpdateDatLichAsync(DatLich datLich);
        Task<bool> UpdateTrangThaiAsync(int iddatLich, TrangThaiDatLich trangThailich);
        Task<List<NhanVien>> GetNhanVienSelectListAsync();
        Task<List<DichVu>> GetDichVuSelectListAsync();
    }
}
