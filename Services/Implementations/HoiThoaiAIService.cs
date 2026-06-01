using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Services.Implementations
{
    public class HoiThoaiAIService : IHoiThoaiAIService
    {
        private readonly AppDbContext _context;
        public HoiThoaiAIService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HoiThoaiAI> GetByIdAsync(int id)
        {
            var data = await _context.HoiThoaiAIs
                .Include(x => x.KhachHang)
                .Include(x => x.NhanVien)
                .Include(x => x.TinNhanAIs)
                .FirstOrDefaultAsync(x => x.IdHoiThoaiAI == id);

            if(data == null)
            {
                throw new Exception("Không tìm thấy hội thoại.");
            }
            return data;
        }

        public async Task<List<HoiThoaiAI>> GetHoiThoaiChoNhanVienAsync()
        {
            return await _context.HoiThoaiAIs
                .Include(x => x.KhachHang)
                .Include(x => x.NhanVien)
                .Include(x => x.TinNhanAIs)
                .Where(x => x.CanNhanVienTuVan
                         && x.TrangThaiHoiThoai == "ChoNhanVien"
                         && x.IdNhanVien == null)
                .OrderByDescending(x => x.ThoiGianBatDau)
                .ToListAsync();
        }

        public async Task<List<HoiThoaiAI>> GetHoiThoaiCuaNhanVienAsync(int idNhanVien)
        {
            return await _context.HoiThoaiAIs
                .Include(x => x.KhachHang)
                .Include(x => x.NhanVien)
                .Include(x => x.TinNhanAIs)
                .Where(x => x.IdNhanVien == idNhanVien
                         && x.TrangThaiHoiThoai == "DaChuyenNhanVien")
                .OrderByDescending(x => x.ThoiGianBatDau)
                .ToListAsync();
        }

        public async Task<List<HoiThoaiAI>> GetTatCaHoiThoaiAsync()
        {
            return await _context.HoiThoaiAIs
                .Include(x => x.KhachHang)
                .Include (x => x.NhanVien)
                .Include (x => x.TinNhanAIs)
                .OrderByDescending( x=> x.ThoiGianBatDau)
                .ToListAsync();
        }

        public async Task KetThucHoiThoaiAsync(int idHoiThoaiAI)
        {
            var hoiThoai = await GetByIdAsync(idHoiThoaiAI);

            hoiThoai.TrangThaiHoiThoai = "DaKetThuc";
            hoiThoai.ThoiGianKetThuc = DateTime.Now;

            var tinNhanSystem = new TinNhanAI
            {
                IdHoiThoaiAI = idHoiThoaiAI,
                VaiTro = "System",
                NoiDung = "Cuộc trò chuyện đã được kết thúc.",
                thoiGianGui = DateTime.Now
            };

            await _context.TinNhanAIs.AddAsync(tinNhanSystem);
            await _context.SaveChangesAsync();
        }

        public async Task NhanVienTraLoiAsync(int idHoiThoaiAI, int idNhanVien, string noiDung)
        {
            if (string.IsNullOrWhiteSpace(noiDung))
            {
                throw new Exception("Vui lòng nhập nội dung trả lời.");
            }

            var hoiThoai = await GetByIdAsync(idHoiThoaiAI);

            if(hoiThoai.IdNhanVien != idNhanVien)
            {
                throw new Exception("Bạn không phải nhân viên đang phụ trách hội thoại này.");
            }

            if(hoiThoai.TrangThaiHoiThoai == "DaKetThuc")
            {
                throw new Exception("Hội thoại đã kết thúc.");
            }

            var tinNhan = new TinNhanAI
            {
                IdHoiThoaiAI = idHoiThoaiAI,
                VaiTro = "NhanVien",
                NoiDung = noiDung,
                thoiGianGui = DateTime.Now
            };

            await _context.TinNhanAIs.AddAsync(tinNhan);
            await _context.SaveChangesAsync();
        }

        public async Task TiepNhanAsync(int idHoiThoaiAI, int idNhanVien)
        {
            var hoiThoai = await GetByIdAsync(idHoiThoaiAI);

            if(hoiThoai.IdNhanVien != null)
            {
                throw new Exception("Hội thoại này đã có nhân viên tiếp nhận.");
            }

            if(hoiThoai.TrangThaiHoiThoai != "ChoNhanVien")
            {
                throw new Exception("Hội thoại này không ở trạng thái chờ nhân viên.");
            }           

            hoiThoai.IdNhanVien = idNhanVien;
            hoiThoai.CanNhanVienTuVan = true;
            hoiThoai.TrangThaiHoiThoai = "DaChuyenNhanVien";

            var tinNhanSystem = new TinNhanAI
            {
                IdHoiThoaiAI = idHoiThoaiAI,
                VaiTro = "System",
                NoiDung = "Nhân viên đã tiếp nhận cuộc trò chuyện",
                thoiGianGui = DateTime.Now
            };

            await _context.TinNhanAIs.AddAsync(tinNhanSystem);
            await _context.SaveChangesAsync();
        }
    }
}
