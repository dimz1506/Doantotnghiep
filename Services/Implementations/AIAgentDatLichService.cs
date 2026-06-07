using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Doantotnghiep.Services.Implementations
{
    public class AIAgentDatLichService : IAIAgentDatLichService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AIAgentDatLichService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Task<bool> LaYeuCauDatLichAsync(string noiDung)
        {
            var text = noiDung.ToLower();

            var keywords = new[]
            {
                "đặt lịch",
                "dat lich",
                "muốn đặt",
                "muon dat",
                "book lịch",
                "book lich",
                "lịch hẹn",
                "lich hen",
                "làm dịch vụ",
                "lam dich vu",
                "hủy lịch",
                "huy lich",
                "hủy lịch hẹn",
                "huy lich hen",
                "hủy cuộc hẹn",
                "huy cuoc hen"
            };

            return Task.FromResult(keywords.Any(x => text.Contains(x)));
        }

        public async Task<string> XuLyDatLichBangAIAsync(
            HoiThoaiAI hoiThoai,
            int? idKhachHang,
            string noiDungKhach)
        {
            if (idKhachHang == null)
            {
                return "Để đặt lịch, anh/chị vui lòng đăng nhập tài khoản khách hàng trước ạ.";
            }

            var state = LayState(hoiThoai);
            state.DangDatLich = true;

            var text = noiDungKhach.ToLower().Trim();
            var aiResult = await PhanTichYeuCauDatLichBangAIAsync(noiDungKhach);

            if (LaHuyDatLich(text) || aiResult?.MuonHuy == true)
            {
                hoiThoai.GhiChuNoiBo = null;
                await _context.SaveChangesAsync();

                return "Em đã hủy quá trình đặt lịch bằng AI. Khi cần đặt lại, anh/chị cứ nhắn em nhé.";
            }

            if ((LaXacNhan(text) || aiResult?.XacNhan == true) && state.DangChoXacNhanHuy)
            {
                return await HuyLichSauKhiXacNhanAsync(hoiThoai, idKhachHang.Value, state);
            }

            if ((LaXacNhan(text) || aiResult?.XacNhan == true) && state.DangChoXacNhan)
            {
                return await TaoLichSauKhiXacNhanAsync(hoiThoai, idKhachHang.Value, state);
            }

            if (aiResult?.MuonHuyLichDaDat == true || LaYeuCauHuyLichDaDat(text))
            {
                return await XuLyYeuCauHuyLichDaDatAsync(
                    hoiThoai,
                    idKhachHang.Value,
                    state,
                    aiResult
                );
            }

            var dichVuTrongTinNhan = await TimDichVuPhuHopAsync(aiResult?.TenDichVu ?? noiDungKhach);

            if (dichVuTrongTinNhan != null)
            {
                if (state.IdDichVu == null || state.IdDichVu.Value != dichVuTrongTinNhan.IdDichVu)
                {
                    state.IdDichVu = dichVuTrongTinNhan.IdDichVu;
                    state.TenDichVu = dichVuTrongTinNhan.TenDichVu;

                    state.IdNhanVien = null;
                    state.TenNhanVien = null;
                    state.DangChoXacNhan = false;
                }
            }

            DateTime? ngayTrongTinNhan = null;

            if (!string.IsNullOrWhiteSpace(aiResult?.NgayHen)
                && DateTime.TryParse(aiResult.NgayHen, out var ngayAI))
            {
                ngayTrongTinNhan = ngayAI.Date;
            }
            else
            {
                ngayTrongTinNhan = DocNgayHen(noiDungKhach);
            }

            if (ngayTrongTinNhan.HasValue)
            {
                if (state.NgayHen == null || state.NgayHen.Value.Date != ngayTrongTinNhan.Value.Date)
                {
                    state.NgayHen = ngayTrongTinNhan.Value.Date;

                    state.IdNhanVien = null;
                    state.TenNhanVien = null;
                    state.DangChoXacNhan = false;
                }
            }

            TimeSpan? gioTrongTinNhan = null;

            if (!string.IsNullOrWhiteSpace(aiResult?.GioBatDau)
                && TimeSpan.TryParse(aiResult.GioBatDau, out var gioAI))
            {
                gioTrongTinNhan = gioAI;
            }
            else
            {
                gioTrongTinNhan = DocGioBatDau(noiDungKhach);
            }

            if (gioTrongTinNhan.HasValue)
            {
                if (state.GioBatDau == null || state.GioBatDau.Value != gioTrongTinNhan.Value)
                {
                    state.GioBatDau = gioTrongTinNhan.Value;

                    state.IdNhanVien = null;
                    state.TenNhanVien = null;
                    state.DangChoXacNhan = false;
                }
            }

            if (!state.IdDichVu.HasValue)
            {
                LuuState(hoiThoai, state);
                await _context.SaveChangesAsync();

                return "Anh/chị muốn đặt dịch vụ nào ạ? Ví dụ: massage body, chăm sóc da, gội đầu dưỡng sinh...";
            }

            if (!state.NgayHen.HasValue)
            {
                LuuState(hoiThoai, state);
                await _context.SaveChangesAsync();

                return $"Anh/chị muốn đặt dịch vụ {state.TenDichVu} vào ngày nào ạ? Ví dụ: hôm nay, ngày mai, 05/06.";
            }

            var dichVuDaChon = await _context.DichVus
                .FirstOrDefaultAsync(x =>
                    x.IdDichVu == state.IdDichVu.Value &&
                    x.TrangThaiDV == true &&
                    !x.IsDeleted);

            if (dichVuDaChon == null)
            {
                hoiThoai.GhiChuNoiBo = null;
                await _context.SaveChangesAsync();

                return "Dịch vụ đã chọn không còn hoạt động. Anh/chị vui lòng chọn dịch vụ khác.";
            }

            if (!state.GioBatDau.HasValue)
            {
                var gioTrong = await GoiYGioTrongAsync(
                    dichVuDaChon,
                    state.NgayHen.Value,
                    noiDungKhach
                );

                LuuState(hoiThoai, state);
                await _context.SaveChangesAsync();

                if (gioTrong.Any())
                {
                    return
                        $"Dịch vụ {state.TenDichVu} ngày {state.NgayHen:dd/MM/yyyy} hiện còn các khung giờ trống:\n\n" +
                        string.Join("\n", gioTrong.Select(x => $"- {x}")) +
                        "\n\nAnh/chị muốn chọn khung giờ nào ạ?";
                }

                return
                    $"Hiện ngày {state.NgayHen:dd/MM/yyyy} chưa tìm thấy khung giờ trống cho dịch vụ {state.TenDichVu}. " +
                    $"Anh/chị muốn chọn ngày khác không ạ?";
            }

            var gioBatDau = state.NgayHen.Value.Date.Add(state.GioBatDau.Value);
            var gioKetThuc = gioBatDau.AddMinutes(dichVuDaChon.ThoiLuongDV);

            var nhanVienPhuHop = await TimNhanVienPhuHopAsync(
                dichVuDaChon,
                state.NgayHen.Value,
                gioBatDau,
                gioKetThuc
            );

            if (nhanVienPhuHop == null)
            {
                var gioTrong = await GoiYGioTrongAsync(
                    dichVuDaChon,
                    state.NgayHen.Value,
                    noiDungKhach
                );

                state.GioBatDau = null;
                state.IdNhanVien = null;
                state.TenNhanVien = null;
                state.DangChoXacNhan = false;

                LuuState(hoiThoai, state);
                await _context.SaveChangesAsync();

                if (gioTrong.Any())
                {
                    return
                        $"Khung giờ anh/chị chọn hiện chưa có nhân viên trống.\n\n" +
                        $"Dịch vụ {state.TenDichVu} ngày {state.NgayHen:dd/MM/yyyy} còn các khung giờ phù hợp:\n\n" +
                        string.Join("\n", gioTrong.Select(x => $"- {x}")) +
                        "\n\nAnh/chị muốn chọn khung giờ nào ạ?";
                }

                return
                    $"Hiện tại chưa tìm thấy nhân viên trống cho dịch vụ {state.TenDichVu} vào ngày {state.NgayHen:dd/MM/yyyy}. " +
                    $"Anh/chị vui lòng chọn ngày khác hoặc em có thể chuyển sang nhân viên tư vấn hỗ trợ thêm.";
            }

            state.IdNhanVien = nhanVienPhuHop.IdNhanVien;
            state.TenNhanVien = nhanVienPhuHop.TenNhanVien;
            state.DangChoXacNhan = true;
            state.DangChoXacNhanHuy = false;
            state.IdDatLichCanHuy = null;

            LuuState(hoiThoai, state);
            await _context.SaveChangesAsync();

            return
                $"Em tìm thấy lịch phù hợp cho anh/chị:\n\n" +
                $"- Dịch vụ: {state.TenDichVu}\n" +
                $"- Ngày hẹn: {state.NgayHen:dd/MM/yyyy}\n" +
                $"- Giờ bắt đầu: {state.GioBatDau:hh\\:mm}\n" +
                $"- Giờ kết thúc dự kiến: {gioKetThuc:HH:mm}\n" +
                $"- Nhân viên phụ trách: {state.TenNhanVien}\n\n" +
                $"Anh/chị nhắn \"xác nhận\" để em tạo lịch hẹn ạ. Nếu muốn hủy, nhắn \"hủy đặt lịch\".";
        }

        private async Task<string> TaoLichSauKhiXacNhanAsync(
            HoiThoaiAI hoiThoai,
            int idKhachHang,
            AIDatLichState state)
        {
            if (!state.IdDichVu.HasValue ||
                !state.NgayHen.HasValue ||
                !state.GioBatDau.HasValue ||
                !state.IdNhanVien.HasValue)
            {
                state.DangChoXacNhan = false;
                LuuState(hoiThoai, state);
                await _context.SaveChangesAsync();

                return "Thông tin đặt lịch chưa đầy đủ. Anh/chị vui lòng cung cấp lại dịch vụ, ngày và giờ hẹn.";
            }

            var dichVu = await _context.DichVus
                .FirstOrDefaultAsync(x =>
                    x.IdDichVu == state.IdDichVu.Value &&
                    x.TrangThaiDV == true &&
                    !x.IsDeleted);

            if (dichVu == null)
            {
                return "Không tìm thấy dịch vụ để đặt lịch.";
            }

            var gioBatDau = state.NgayHen.Value.Date.Add(state.GioBatDau.Value);
            var gioKetThuc = gioBatDau.AddMinutes(dichVu.ThoiLuongDV);

            var conRanh = await KiemTraNhanVienRanhAsync(
                state.IdNhanVien.Value,
                state.NgayHen.Value,
                gioBatDau,
                gioKetThuc
            );

            if (!conRanh)
            {
                state.DangChoXacNhan = false;
                state.IdNhanVien = null;
                state.TenNhanVien = null;
                LuuState(hoiThoai, state);
                await _context.SaveChangesAsync();

                return "Rất tiếc, khung giờ này vừa có người đặt mất rồi. Anh/chị vui lòng chọn giờ khác ạ.";
            }

            var datLich = new DatLich
            {
                IdKhachHang = idKhachHang,
                IdNhanVien = state.IdNhanVien.Value,
                NgayHenLich = state.NgayHen.Value,
                GioBatDauDV = gioBatDau,
                GioKetThucDV = gioKetThuc,
                TrangThaiDatLich = TrangThaiDatLich.ChoXacNhan,
                GhiChuDatLich = "Đặt lịch bằng AI Agent",
                NgayTaoDatLich = DateTime.Now
            };

            await _context.DatLichs.AddAsync(datLich);
            await _context.SaveChangesAsync();

            var chiTiet = new ChiTietDatLich
            {
                IdDatLich = datLich.IdDatLich,
                IdDichVu = dichVu.IdDichVu,
                SoLuong = 1,
                GiaDichVu = dichVu.GiaDichVu
            };

            await _context.ChiTietDatLiches.AddAsync(chiTiet);

            var thongBao = new ThongBao
            {
                IdKhachHang = idKhachHang,
                TieuDe = "Đặt lịch thành công",
                NoiDung =
                    $"Bạn đã đặt lịch thành công.\n" +
                    $"Dịch vụ: {dichVu.TenDichVu}\n" +
                    $"Ngày hẹn: {datLich.NgayHenLich:dd/MM/yyyy}\n" +
                    $"Giờ bắt đầu: {datLich.GioBatDauDV:HH:mm}\n" +
                    $"Giờ kết thúc dự kiến: {datLich.GioKetThucDV:HH:mm}\n" +
                    $"Nhân viên phụ trách: {state.TenNhanVien}\n" +
                    $"Trạng thái: Chờ xác nhận",
                DuongDan = $"/DatLich/Details/{datLich.IdDatLich}",
                DaDoc = false,
                NgayTao = DateTime.Now
            };

            await _context.ThongBaos.AddAsync(thongBao);

            hoiThoai.GhiChuNoiBo = null;
            await _context.SaveChangesAsync();

            return
                $"Đặt lịch thành công ạ!\n\n" +
                $"- Dịch vụ: {dichVu.TenDichVu}\n" +
                $"- Ngày hẹn: {state.NgayHen:dd/MM/yyyy}\n" +
                $"- Giờ bắt đầu: {state.GioBatDau:hh\\:mm}\n" +
                $"- Giờ kết thúc: {gioKetThuc:HH:mm}\n" +
                $"- Trạng thái: Chờ xác nhận\n\n" +
                $"Em cũng đã gửi thông báo vào mục Thông báo của anh/chị.";
        }

        private async Task<string> XuLyYeuCauHuyLichDaDatAsync(
            HoiThoaiAI hoiThoai,
            int idKhachHang,
            AIDatLichState state,
            AIYeuCauDatLichResult? aiResult)
        {
            DateTime? ngayHen = null;
            TimeSpan? gioBatDau = null;

            if (!string.IsNullOrWhiteSpace(aiResult?.NgayHen)
                && DateTime.TryParse(aiResult.NgayHen, out var ngayAI))
            {
                ngayHen = ngayAI.Date;
            }

            if (!string.IsNullOrWhiteSpace(aiResult?.GioBatDau)
                && TimeSpan.TryParse(aiResult.GioBatDau, out var gioAI))
            {
                gioBatDau = gioAI;
            }

            IQueryable<DatLich> query = _context.DatLichs
                .Include(x => x.ChiTietDatLichs)
                    .ThenInclude(x => x.DichVu)
                .Where(x =>
                    x.IdKhachHang == idKhachHang &&
                    x.TrangThaiDatLich != TrangThaiDatLich.DaHuy &&
                    x.NgayHenLich.Date >= DateTime.Today);

            if (ngayHen.HasValue)
            {
                query = query.Where(x => x.NgayHenLich.Date == ngayHen.Value.Date);
            }

            if (gioBatDau.HasValue)
            {
                query = query.Where(x =>
                    x.GioBatDauDV.Hour == gioBatDau.Value.Hours &&
                    x.GioBatDauDV.Minute == gioBatDau.Value.Minutes);
            }

            if (!string.IsNullOrWhiteSpace(aiResult?.TenDichVu))
            {
                var tenDichVu = aiResult.TenDichVu.ToLower();

                query = query.Where(x =>
                    x.ChiTietDatLichs.Any(ct =>
                        ct.DichVu != null &&
                        ct.DichVu.TenDichVu.ToLower().Contains(tenDichVu)));
            }

            var lich = await query
                .OrderBy(x => x.NgayHenLich)
                .ThenBy(x => x.GioBatDauDV)
                .FirstOrDefaultAsync();

            if (lich == null)
            {
                return "Em chưa tìm thấy lịch hẹn phù hợp để hủy. Anh/chị vui lòng cung cấp rõ dịch vụ, ngày và giờ của lịch muốn hủy ạ.";
            }

            var soGioConLai = (lich.GioBatDauDV - DateTime.Now).TotalHours;

            if (soGioConLai < 6)
            {
                return
                    $"Lịch hẹn này còn dưới 6 giờ nữa là đến giờ hẹn nên hệ thống không cho hủy tự động ạ.\n\n" +
                    $"- Ngày hẹn: {lich.NgayHenLich:dd/MM/yyyy}\n" +
                    $"- Giờ bắt đầu: {lich.GioBatDauDV:HH:mm}\n\n" +
                    $"Anh/chị vui lòng liên hệ nhân viên để được hỗ trợ thêm.";
            }

            var tenDichVuTimDuoc = lich.ChiTietDatLichs.FirstOrDefault()?.DichVu?.TenDichVu ?? "Dịch vụ";

            state.DangChoXacNhanHuy = true;
            state.IdDatLichCanHuy = lich.IdDatLich;

            state.DangChoXacNhan = false;
            state.DangDatLich = false;

            LuuState(hoiThoai, state);
            await _context.SaveChangesAsync();

            return
                $"Em tìm thấy lịch hẹn cần hủy:\n\n" +
                $"- Dịch vụ: {tenDichVuTimDuoc}\n" +
                $"- Ngày hẹn: {lich.NgayHenLich:dd/MM/yyyy}\n" +
                $"- Giờ bắt đầu: {lich.GioBatDauDV:HH:mm}\n" +
                $"- Trạng thái: {lich.TrangThaiDatLich}\n\n" +
                $"Lịch này còn trước giờ hẹn trên 6 giờ nên có thể hủy.\n" +
                $"Anh/chị nhắn \"xác nhận hủy\" để em hủy lịch này ạ.";
        }

        private async Task<string> HuyLichSauKhiXacNhanAsync(
            HoiThoaiAI hoiThoai,
            int idKhachHang,
            AIDatLichState state)
        {
            if (!state.IdDatLichCanHuy.HasValue)
            {
                state.DangChoXacNhanHuy = false;
                LuuState(hoiThoai, state);
                await _context.SaveChangesAsync();

                return "Em chưa xác định được lịch cần hủy. Anh/chị vui lòng gửi lại thông tin lịch hẹn.";
            }

            var lich = await _context.DatLichs
                .Include(x => x.ChiTietDatLichs)
                    .ThenInclude(x => x.DichVu)
                .FirstOrDefaultAsync(x =>
                    x.IdDatLich == state.IdDatLichCanHuy.Value &&
                    x.IdKhachHang == idKhachHang);

            if (lich == null)
            {
                hoiThoai.GhiChuNoiBo = null;
                await _context.SaveChangesAsync();

                return "Không tìm thấy lịch hẹn cần hủy.";
            }

            var soGioConLai = (lich.GioBatDauDV - DateTime.Now).TotalHours;

            if (soGioConLai < 6)
            {
                hoiThoai.GhiChuNoiBo = null;
                await _context.SaveChangesAsync();

                return "Lịch hẹn này còn dưới 6 giờ nữa là đến giờ hẹn nên không thể hủy tự động. Anh/chị vui lòng liên hệ nhân viên để được hỗ trợ.";
            }

            lich.TrangThaiDatLich = TrangThaiDatLich.DaHuy;
            lich.NgayCapNhatDatLich = DateTime.Now;

            var tenDichVu = lich.ChiTietDatLichs.FirstOrDefault()?.DichVu?.TenDichVu ?? "Dịch vụ";

            var thongBao = new ThongBao
            {
                IdKhachHang = idKhachHang,
                TieuDe = "Hủy lịch thành công",
                NoiDung =
                    $"Bạn đã hủy lịch thành công.\n" +
                    $"Dịch vụ: {tenDichVu}\n" +
                    $"Ngày hẹn: {lich.NgayHenLich:dd/MM/yyyy}\n" +
                    $"Giờ bắt đầu: {lich.GioBatDauDV:HH:mm}",
                DuongDan = $"/DatLich/Details/{lich.IdDatLich}",
                DaDoc = false,
                NgayTao = DateTime.Now
            };

            await _context.ThongBaos.AddAsync(thongBao);

            state.DangChoXacNhanHuy = false;
            state.IdDatLichCanHuy = null;
            hoiThoai.GhiChuNoiBo = null;

            await _context.SaveChangesAsync();

            return
                $"Em đã hủy lịch hẹn thành công ạ.\n\n" +
                $"- Dịch vụ: {tenDichVu}\n" +
                $"- Ngày hẹn: {lich.NgayHenLich:dd/MM/yyyy}\n" +
                $"- Giờ bắt đầu: {lich.GioBatDauDV:HH:mm}\n\n" +
                $"Em cũng đã gửi thông báo hủy lịch vào mục Thông báo của anh/chị.";
        }

        private async Task<DichVu?> TimDichVuPhuHopAsync(string noiDung)
        {
            var text = noiDung.ToLower();

            var dichVus = await _context.DichVus
                .Where(x => x.TrangThaiDV == true && !x.IsDeleted)
                .ToListAsync();

            return dichVus
                .Select(x => new
                {
                    DichVu = x,
                    Diem = TinhDiemDichVu(text, x)
                })
                .Where(x => x.Diem > 0)
                .OrderByDescending(x => x.Diem)
                .Select(x => x.DichVu)
                .FirstOrDefault();
        }

        private int TinhDiemDichVu(string text, DichVu dichVu)
        {
            var diem = 0;
            var ten = dichVu.TenDichVu.ToLower();

            if (text.Contains(ten))
            {
                diem += 20;
            }

            foreach (var word in ten.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (word.Length >= 3 && text.Contains(word))
                {
                    diem += 5;
                }
            }

            if (!string.IsNullOrWhiteSpace(dichVu.MoTaDichVu))
            {
                var moTa = dichVu.MoTaDichVu.ToLower();

                foreach (var word in moTa.Split(' ', StringSplitOptions.RemoveEmptyEntries).Distinct())
                {
                    if (word.Length >= 4 && text.Contains(word))
                    {
                        diem += 1;
                    }
                }
            }

            return diem;
        }

        private DateTime? DocNgayHen(string noiDung)
        {
            var text = noiDung.ToLower();

            if (text.Contains("hôm nay"))
            {
                return DateTime.Today;
            }

            if (text.Contains("ngày mai") || text.Contains("mai"))
            {
                return DateTime.Today.AddDays(1);
            }

            if (text.Contains("ngày kia") || text.Contains("mốt"))
            {
                return DateTime.Today.AddDays(2);
            }

            var match = Regex.Match(text, @"(\d{1,2})[\/\-](\d{1,2})([\/\-](\d{4}))?");

            if (match.Success)
            {
                var ngay = int.Parse(match.Groups[1].Value);
                var thang = int.Parse(match.Groups[2].Value);
                var nam = match.Groups[4].Success
                    ? int.Parse(match.Groups[4].Value)
                    : DateTime.Today.Year;

                try
                {
                    var ngayHen = new DateTime(nam, thang, ngay);

                    if (ngayHen.Date < DateTime.Today.Date)
                    {
                        ngayHen = ngayHen.AddYears(1);
                    }

                    return ngayHen;
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        private TimeSpan? DocGioBatDau(string noiDung)
        {
            var text = noiDung.ToLower();

            var match1 = Regex.Match(text, @"(\d{1,2})h(\d{1,2})?");

            if (match1.Success)
            {
                var gio = int.Parse(match1.Groups[1].Value);
                var phut = match1.Groups[2].Success && !string.IsNullOrWhiteSpace(match1.Groups[2].Value)
                    ? int.Parse(match1.Groups[2].Value)
                    : 0;

                if (gio >= 0 && gio <= 23 && phut >= 0 && phut <= 59)
                {
                    return new TimeSpan(gio, phut, 0);
                }
            }

            var match2 = Regex.Match(text, @"(\d{1,2}):(\d{1,2})");

            if (match2.Success)
            {
                var gio = int.Parse(match2.Groups[1].Value);
                var phut = int.Parse(match2.Groups[2].Value);

                if (gio >= 0 && gio <= 23 && phut >= 0 && phut <= 59)
                {
                    return new TimeSpan(gio, phut, 0);
                }
            }

            return null;
        }

        private async Task<NhanVien?> TimNhanVienPhuHopAsync(
     DichVu dichVu,
     DateTime ngayHen,
     DateTime gioBatDau,
     DateTime gioKetThuc)
        {
            var nhanViens = await _context.NhanViens
                .Include(x => x.TaiKhoan)
                    .ThenInclude(x => x.VaiTro)
                .Where(x =>
                    x.TrangThaiNV == TrangThaiNhanVien.DangLamViec &&
                    !x.IsDeleted &&
                    x.TaiKhoan != null &&
                    x.TaiKhoan.VaiTro != null &&
                    x.TaiKhoan.VaiTro.TenVaiTro == "NhanVien")
                .ToListAsync();

            foreach (var nv in nhanViens)
            {
                var ranh = await KiemTraNhanVienRanhAsync(
                    nv.IdNhanVien,
                    ngayHen,
                    gioBatDau,
                    gioKetThuc
                );

                if (ranh)
                {
                    return nv;
                }
            }

            return null;
        }

        private async Task<bool> KiemTraNhanVienRanhAsync(
     int idNhanVien,
     DateTime ngayHen,
     DateTime gioBatDau,
     DateTime gioKetThuc)
        {
            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(x =>
                    x.IdNhanVien == idNhanVien &&
                    x.TrangThaiNV == TrangThaiNhanVien.DangLamViec &&
                    !x.IsDeleted);

            if (nhanVien == null)
            {
                return false;
            }

            if (!nhanVien.LaNhanVienFullTime)
            {
                return false;
            }

            if (gioBatDau.TimeOfDay < nhanVien.GioBatDauLamViec ||
                gioKetThuc.TimeOfDay > nhanVien.GioKetThucLamViec)
            {
                return false;
            }

            var ngayBatDau = ngayHen.Date;
            var ngayKetThuc = ngayHen.Date.AddDays(1);

            var lichDatCungNgay = await _context.DatLichs
                .Where(x =>
                    x.IdNhanVien == idNhanVien &&
                    x.NgayHenLich >= ngayBatDau &&
                    x.NgayHenLich < ngayKetThuc &&
                    x.TrangThaiDatLich != TrangThaiDatLich.DaHuy)
                .ToListAsync();

            var biTrungLich = lichDatCungNgay.Any(x =>
                gioBatDau < x.GioKetThucDV &&
                gioKetThuc > x.GioBatDauDV
            );

            return !biTrungLich;
        }

        private async Task<List<string>> GoiYGioTrongAsync(
            DichVu dichVu,
            DateTime ngayHen,
            string noiDungKhach)
        {
            var text = noiDungKhach.ToLower();

            List<TimeSpan> khungGioCanCheck;

            if (text.Contains("sáng") || text.Contains("sang"))
            {
                khungGioCanCheck = new List<TimeSpan>
                {
                    new TimeSpan(8, 0, 0),
                    new TimeSpan(8, 30, 0),
                    new TimeSpan(9, 0, 0),
                    new TimeSpan(9, 30, 0),
                    new TimeSpan(10, 0, 0),
                    new TimeSpan(10, 30, 0)
                };
            }
            else if (text.Contains("chiều") || text.Contains("chieu"))
            {
                khungGioCanCheck = new List<TimeSpan>
                {
                    new TimeSpan(13, 30, 0),
                    new TimeSpan(14, 0, 0),
                    new TimeSpan(14, 30, 0),
                    new TimeSpan(15, 0, 0),
                    new TimeSpan(15, 30, 0),
                    new TimeSpan(16, 0, 0),
                    new TimeSpan(16, 30, 0)
                };
            }
            else
            {
                khungGioCanCheck = new List<TimeSpan>
                {
                    new TimeSpan(8, 0, 0),
                    new TimeSpan(8, 30, 0),
                    new TimeSpan(9, 0, 0),
                    new TimeSpan(9, 30, 0),
                    new TimeSpan(10, 0, 0),
                    new TimeSpan(10, 30, 0),
                    new TimeSpan(13, 30, 0),
                    new TimeSpan(14, 0, 0),
                    new TimeSpan(14, 30, 0),
                    new TimeSpan(15, 0, 0),
                    new TimeSpan(15, 30, 0),
                    new TimeSpan(16, 0, 0),
                    new TimeSpan(16, 30, 0)
                };
            }

            var gioTrong = new List<string>();

            foreach (var gio in khungGioCanCheck)
            {
                var gioBatDau = ngayHen.Date.Add(gio);
                var gioKetThuc = gioBatDau.AddMinutes(dichVu.ThoiLuongDV);

                var nhanVienPhuHop = await TimNhanVienPhuHopAsync(
                    dichVu,
                    ngayHen,
                    gioBatDau,
                    gioKetThuc
                );

                if (nhanVienPhuHop != null)
                {
                    gioTrong.Add(gio.ToString(@"hh\:mm"));
                }

                if (gioTrong.Count >= 5)
                {
                    break;
                }
            }

            return gioTrong;
        }

        private bool LaXacNhan(string text)
        {
            text = text.ToLower();

            return text.Contains("xác nhận")
                || text.Contains("xac nhan")
                || text.Contains("đồng ý")
                || text.Contains("dong y")
                || text == "ok"
                || text == "oke"
                || text.Contains("đặt đi");
        }

        private bool LaHuyDatLich(string text)
        {
            text = text.ToLower().Trim();

            var tuKhoaHuy = new[]
            {
                "hủy đặt lịch",
                "huy dat lich",
                "không đặt nữa",
                "khong dat nua",
                "không muốn đặt nữa",
                "khong muon dat nua",
                "thôi không đặt",
                "thoi khong dat",
                "bỏ đặt lịch",
                "bo dat lich"
            };

            return tuKhoaHuy.Any(x => text.Contains(x));
        }

        private bool LaYeuCauHuyLichDaDat(string text)
        {
            var tuKhoa = new[]
            {
                "hủy lịch đã đặt",
                "huy lich da dat",
                "hủy lịch hẹn",
                "huy lich hen",
                "hủy lịch ngày",
                "huy lich ngay",
                "muốn hủy lịch",
                "muon huy lich",
                "hủy cuộc hẹn",
                "huy cuoc hen"
            };

            return tuKhoa.Any(x => text.Contains(x));
        }

        private AIDatLichState LayState(HoiThoaiAI hoiThoai)
        {
            if (string.IsNullOrWhiteSpace(hoiThoai.GhiChuNoiBo))
            {
                return new AIDatLichState();
            }

            try
            {
                return JsonSerializer.Deserialize<AIDatLichState>(hoiThoai.GhiChuNoiBo)
                       ?? new AIDatLichState();
            }
            catch
            {
                return new AIDatLichState();
            }
        }

        private void LuuState(HoiThoaiAI hoiThoai, AIDatLichState state)
        {
            hoiThoai.GhiChuNoiBo = JsonSerializer.Serialize(state);
        }

        private async Task<string> GoiGeminiAsync(string prompt)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            var model = _configuration["Gemini:Model"] ?? "gemini-2.5-flash";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return "";
            }

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("x-goog-api-key", apiKey);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();

            var response = await httpClient.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return "";
            }

            using var doc = JsonDocument.Parse(responseText);

            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "";
        }

        private async Task<AIYeuCauDatLichResult?> PhanTichYeuCauDatLichBangAIAsync(string noiDungKhach)
        {
            var today = DateTime.Today.ToString("yyyy-MM-dd");

            var prompt = $@"
Bạn là AI Agent phân tích yêu cầu đặt lịch spa.

Hôm nay là: {today}

Nhiệm vụ:
Phân tích câu khách hàng và trả về JSON duy nhất.

Quy tắc:
- Chỉ trả JSON, không giải thích.
- Nếu khách muốn đặt lịch, CoPhaiDatLich = true.
- Nếu khách xác nhận đặt lịch hoặc xác nhận hủy, XacNhan = true.
- Nếu khách muốn hủy quá trình đặt lịch đang nhập nhưng chưa tạo lịch, MuonHuy = true.
- Nếu khách muốn hủy lịch đã đặt, MuonHuyLichDaDat = true.
- NgayHen trả dạng yyyy-MM-dd.
- GioBatDau trả dạng HH:mm.
- Nếu thiếu thông tin thì để null.

Câu khách hàng:
{noiDungKhach}

JSON mẫu:
{{
  ""CoPhaiDatLich"": true,
  ""TenDichVu"": ""massage body"",
  ""NgayHen"": ""2026-05-31"",
  ""GioBatDau"": ""15:00"",
  ""MuonHuy"": false,
  ""MuonHuyLichDaDat"": false,
  ""XacNhan"": false
}}
";

            var jsonText = await GoiGeminiAsync(prompt);

            if (string.IsNullOrWhiteSpace(jsonText))
            {
                return null;
            }

            jsonText = jsonText
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            try
            {
                return JsonSerializer.Deserialize<AIYeuCauDatLichResult>(
                    jsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                return null;
            }
        }
    }
}