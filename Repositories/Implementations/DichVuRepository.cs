using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace Doantotnghiep.Repositories.Implementations
{
    public class DichVuRepository : IDichVuRepository
    {
        private readonly Data.AppDbContext _context;
        public DichVuRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool ok, string error)> CreateDichVuAsync(DichVu dichVu)
        {

            try
            {
                _context.DichVus.Add(dichVu);
                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool ok, string error)> DeleteDichVuAsync(int id)
        {
            try
            {
                _context.DichVus.Remove(_context.DichVus.Find(id));
                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<List<DichVu>> GetAllDichVuAsync(string? searchString, int? idLoaiDichVu)
        {
            try
            {
                var query = _context.DichVus
                    .AsNoTracking()
                    .Include(dv => dv.LoaiDichVu)
                    .Where(dv => !dv.IsDeleted)
                    .AsQueryable();

                if (idLoaiDichVu.HasValue)
                {
                    query = query.Where(dv => dv.IdLoaiDichVu == idLoaiDichVu.Value);
                }

                var data = await query.ToListAsync();

                if (string.IsNullOrWhiteSpace(searchString))
                {
                    return data;
                }

                var keyword = ChuanHoaTuKhoa(searchString);

                var ketQua = data
                    .Select(dv => new
                    {
                        DichVu = dv,
                        Diem = TinhDiemGanDung(keyword, dv)
                    })
                    .Where(x => x.Diem >= 30)
                    .OrderByDescending(x => x.Diem)
                    .Select(x => x.DichVu)
                    .ToList();

                return ketQua;
            }
            catch
            {
                return new List<DichVu>();
            }
        }
        public async Task<DichVu> GetDichVuByIdAsync(int id)
        {
            try
            {
                return await _context.DichVus.Include(dv => dv.LoaiDichVu).FirstOrDefaultAsync(dv => dv.IdDichVu == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<(bool ok, string error)> UpdateDichVuAsync(DichVu dichVu)
        {
            try
            {
                var dichVuCu = await _context.DichVus
                    .FirstOrDefaultAsync(x => x.IdDichVu == dichVu.IdDichVu);

                if (dichVuCu == null)
                {
                    return (false, "Không tìm thấy dịch vụ cần cập nhật.");
                }

                dichVuCu.IdLoaiDichVu = dichVu.IdLoaiDichVu;
                dichVuCu.TenDichVu = dichVu.TenDichVu;
                dichVuCu.MoTaDichVu = dichVu.MoTaDichVu;

                dichVuCu.LieuTrinhDichVu = dichVu.LieuTrinhDichVu;
                dichVuCu.NguyenLieuSuDung = dichVu.NguyenLieuSuDung;
                dichVuCu.QuyTrinhThucHien = dichVu.QuyTrinhThucHien;
                dichVuCu.CongCuSuDung = dichVu.CongCuSuDung;
                dichVuCu.LuuYKhachHang = dichVu.LuuYKhachHang;

                dichVuCu.GiaDichVu = dichVu.GiaDichVu;
                dichVuCu.ThoiLuongDV = dichVu.ThoiLuongDV;
                dichVuCu.TrangThaiDV = dichVu.TrangThaiDV;

                if (!string.IsNullOrWhiteSpace(dichVu.HinhAnhDichVu))
                {
                    dichVuCu.HinhAnhDichVu = dichVu.HinhAnhDichVu;
                }

                await _context.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<List<DichVu>> GetDichVusByIdAsync(List<int> id)
        {
            return await _context.DichVus.Where(dv => id.Contains(dv.IdDichVu)).ToListAsync();
        }
       
        private int TinhDiemGanDung(string keyword, DichVu dichVu)
        {
            var ten = ChuanHoaTuKhoa(dichVu.TenDichVu);
            var moTa = ChuanHoaTuKhoa(dichVu.MoTaDichVu);
            var loai = ChuanHoaTuKhoa(dichVu.LoaiDichVu?.TenLoaiDichVu);

            var diem = 0;

            if (ten == keyword)
            {
                diem += 100;
            }

            if (ten.Contains(keyword))
            {
                diem += 90;
            }

            if (keyword.Contains(ten))
            {
                diem += 80;
            }

            diem += TinhDiemTheoTu(keyword, ten, 40);
            diem += TinhDiemTheoTu(keyword, moTa, 15);
            diem += TinhDiemTheoTu(keyword, loai, 20);

            return diem;
        }

        private int TinhDiemTheoTu(string keyword, string text, int diemMoiTu)
        {
            if (string.IsNullOrWhiteSpace(keyword) || string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            var words = keyword
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Length >= 2)
                .Distinct()
                .ToList();

            var diem = 0;

            foreach (var word in words)
            {
                if (text.Contains(word))
                {
                    diem += diemMoiTu;
                }
                else
                {
                    foreach (var textWord in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (TinhDoGiongNhau(word, textWord) >= 0.8)
                        {
                            diem += diemMoiTu / 2;
                            break;
                        }
                    }
                }
            }

            return diem;
        }

        private double TinhDoGiongNhau(string a, string b)
        {
            if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
            {
                return 0;
            }

            var distance = LevenshteinDistance(a, b);
            var maxLength = Math.Max(a.Length, b.Length);

            if (maxLength == 0)
            {
                return 1;
            }

            return 1.0 - (double)distance / maxLength;
        }

        private int LevenshteinDistance(string a, string b)
        {
            var dp = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
            {
                dp[i, 0] = i;
            }

            for (int j = 0; j <= b.Length; j++)
            {
                dp[0, j] = j;
            }

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    var cost = a[i - 1] == b[j - 1] ? 0 : 1;

                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + cost
                    );
                }
            }

            return dp[a.Length, b.Length];
        }

        private string ChuanHoaTuKhoa(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            text = text.Trim().ToLower();

            var normalized = text.Normalize(System.Text.NormalizationForm.FormD);

            var chars = normalized
                .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                    != System.Globalization.UnicodeCategory.NonSpacingMark)
                .ToArray();

            return new string(chars)
                .Normalize(System.Text.NormalizationForm.FormC)
                .Replace("đ", "d")
                .Replace("Đ", "D");
        }
    }
}
