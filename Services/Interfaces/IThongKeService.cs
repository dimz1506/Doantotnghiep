using Doantotnghiep.Models.ViewModel.ThongKe;

namespace Doantotnghiep.Services.Interfaces
{
    public interface IThongKeService
    {
        Task<AdminDashboardViewModel> GetDashBoardAsync();
        Task<List<DoanhThuTheoNgayViewModel>> GetDoanhThuTheoNgayAsync(DateTime tuNgay, DateTime denNgay);
        Task<List<DoanhThuTheoThangViewModel>> GetDoanhThuTheoThangAsync(int nam);
        Task<List<DichVuBanChayViewModel>> GetDichVuBanChayAsync(DateTime? tuNgay, DateTime? denNgay);
        Task<List<LichHenTheoTrangThaiViewModel>> GetLichHenTheoTrangThaiAsync();
        Task<List<NhanVienThongKeViewModel>> GetThongKeNhanVienAsync(DateTime? tuNgay, DateTime? denNgay);
    }
}
