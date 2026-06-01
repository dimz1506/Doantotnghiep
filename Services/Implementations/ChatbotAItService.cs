using Doantotnghiep.Data;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Doantotnghiep.Services.Implementations
{
    public class ChatbotAItService : IChatbotAIService
    {
        private readonly AppDbContext _context;
        private readonly IAIAgentDatLichService _aiIAgentDatLichService;
        private readonly IConfiguration _configuration;

        public ChatbotAItService(AppDbContext context, IAIAgentDatLichService aIAgentDatLichService, IConfiguration configuration)
        {
            _context = context;
            _aiIAgentDatLichService = aIAgentDatLichService;
            _configuration = configuration;
        }
        public async Task<ChatbotViewModel> BatDauChatAsync(int? idKhachHang)
        {
            var hoiThoai = new HoiThoaiAI
            {
                IdKhachHang = idKhachHang,
                ThoiGianBatDau = DateTime.Now,
                TrangThaiHoiThoai = "DangChat",
                CanNhanVienTuVan = false,
                IdNhanVien = null
            };

            await _context.HoiThoaiAIs.AddAsync(hoiThoai);
            await _context.SaveChangesAsync();

            var tinhNhanChao = new TinNhanAI
            {
                IdHoiThoaiAI = hoiThoai.IdHoiThoaiAI,
                VaiTro = "Assistant",
                NoiDung = "Xin chào, em là trợ lý AI của spa. Anh/chị cần tư vấn dịch vụ, hỏi chính sách hay muốn hỗ trợ đặt lịch ạ?",
                thoiGianGui = DateTime.Now
            };

            await _context.TinNhanAIs.AddAsync(tinhNhanChao);
            await _context.SaveChangesAsync();

            return await LayHoiThoaiAsync(hoiThoai.IdHoiThoaiAI);
        }

        public async Task<ChatbotViewModel> GuiTinNhanAsync(int? idHoiThoaiAI, int? idKhachHang, string cauHoi)
        {
            if (string.IsNullOrWhiteSpace(cauHoi))
            {
                throw new Exception("Vui lòng nhập nội dung câu hỏi.");
            }

            HoiThoaiAI hoiThoai;

            if (idHoiThoaiAI == null || idHoiThoaiAI <= 0)
            {
                hoiThoai = new HoiThoaiAI
                {
                    IdKhachHang = idKhachHang,
                    ThoiGianBatDau = DateTime.Now,
                    TrangThaiHoiThoai = "DangChat",
                    CanNhanVienTuVan = false,
                    IdNhanVien = null
                };

                await _context.HoiThoaiAIs.AddAsync(hoiThoai);
                await _context.SaveChangesAsync();
            }
            else
            {
                hoiThoai = await _context.HoiThoaiAIs
                    .FirstOrDefaultAsync(x => x.IdHoiThoaiAI == idHoiThoaiAI.Value);

                if (hoiThoai == null)
                {
                    throw new Exception("Không tìm thấy hội thoại.");
                }
            }

            // 1. Luôn luôn lưu tin nhắn của khách trước
            var tinNhanKhach = new TinNhanAI
            {
                IdHoiThoaiAI = hoiThoai.IdHoiThoaiAI,
                VaiTro = "User",
                NoiDung = cauHoi,
                thoiGianGui = DateTime.Now
            };

            await _context.TinNhanAIs.AddAsync(tinNhanKhach);

            // 2. Nếu hội thoại đã chuyển sang nhân viên thì không cho AI trả lời nữa
            if (hoiThoai.TrangThaiHoiThoai == "ChoNhanVien"
                || hoiThoai.TrangThaiHoiThoai == "DaChuyenNhanVien")
            {
                var tinNhanThongBao = new TinNhanAI
                {
                    IdHoiThoaiAI = hoiThoai.IdHoiThoaiAI,
                    VaiTro = "System",
                    NoiDung = "Tin nhắn của anh/chị đã được gửi đến nhân viên tư vấn. Vui lòng chờ nhân viên phản hồi.",
                    thoiGianGui = DateTime.Now
                };

                await _context.TinNhanAIs.AddAsync(tinNhanThongBao);
                await _context.SaveChangesAsync();

                return await LayHoiThoaiAsync(hoiThoai.IdHoiThoaiAI);
            }

            // 3. Nếu chưa chuyển nhân viên thì chatbot RAG trả lời bình thường
            var cauTraLoi = await TaoCauTraLoiRagAsync(cauHoi, hoiThoai);

            var tinNhanAI = new TinNhanAI
            {
                IdHoiThoaiAI = hoiThoai.IdHoiThoaiAI,
                VaiTro = "Assistant",
                NoiDung = cauTraLoi,
                thoiGianGui = DateTime.Now
            };

            await _context.TinNhanAIs.AddAsync(tinNhanAI);
            await _context.SaveChangesAsync();

            return await LayHoiThoaiAsync(hoiThoai.IdHoiThoaiAI);
        }

        public async Task<ChatbotViewModel> LayHoiThoaiAsync(int idHoiThoaiAI)
        {
            var hoiThoai = await _context.HoiThoaiAIs
                .Include(x => x.TinNhanAIs)
                .FirstOrDefaultAsync(x => x.IdHoiThoaiAI == idHoiThoaiAI);

            if(hoiThoai == null)
            {
                throw new Exception("không tìm thấy hội thoại");
            }

            return new ChatbotViewModel
            {
                IdHoiThoaiAI = hoiThoai.IdHoiThoaiAI,
                TinNhans = hoiThoai.TinNhanAIs
                    .OrderBy(x => x.thoiGianGui)
                    .Select(x => new TinNhanChatViewModel
                    {
                        VaiTro = x.VaiTro,
                        NoiDung = x.NoiDung,
                        ThoiGianGui = x.thoiGianGui
                    })
                    .ToList()
            };
        }

        private async Task<string> TaoCauTraLoiRagAsync(string cauHoi, HoiThoaiAI hoiThoai)
        {
            var cauHoiLower = cauHoi.ToLower();
            if (CanChuyenNhanVien(cauHoiLower))
            {
                hoiThoai.CanNhanVienTuVan = true;
                hoiThoai.TrangThaiHoiThoai = "ChoNhanVien";
                hoiThoai.IdNhanVien = null;

                _context.HoiThoaiAIs.Update(hoiThoai);

                return "Nội dung này cần nhân viên tư vấn kiểm tra thêm để hỗ trợ chính xác hơn. Em đã chuyển yêu cầu của anh chị sang nhân viên tư vấn.";

            }
            if (await _aiIAgentDatLichService.LaYeuCauDatLichAsync(cauHoi)
                 || !string.IsNullOrWhiteSpace(hoiThoai.GhiChuNoiBo))
            {
                return await _aiIAgentDatLichService.XuLyDatLichBangAIAsync(
                    hoiThoai,
                    hoiThoai.IdKhachHang,
                    cauHoi
                );
            }


            var triThucs = await _context.TriThucs
                .Include(x => x.LoaiTriThuc)
                .Where(x => !x.IsDeleted && x.TrangThaiTriThuc)
                .ToListAsync();

            var ketQuaPhuHop = triThucs.Select(x => new
            {
                TriThuc = x,
                Diem = TinhDiemPhuHop(cauHoiLower, x)
            })
                .Where(x => x.Diem > 0)
                 .OrderByDescending(x => x.Diem)
                .Take(3)
                .ToList();

            if (!ketQuaPhuHop.Any())
            {
                hoiThoai.CanNhanVienTuVan = true;
                hoiThoai.TrangThaiHoiThoai = "ChoNhanVien";
                hoiThoai.IdNhanVien = null;

                _context.HoiThoaiAIs.Update(hoiThoai);

                return "Hiện em chưa có đủ dữ liệu để trả lời chính xác câu hỏi này. Em sẽ chuyển anh/chị sang nhân viên tư vấn để được hỗ trợ tốt hơn.";
            }

            var noiDungTriThuc = string.Join("\n\n", ketQuaPhuHop.Select(x =>
     $"- Tiêu đề: {x.TriThuc.TenTriThuc}\n" +
     $"- Loại: {x.TriThuc.LoaiTriThuc?.TenLoaiTriThuc}\n" +
     $"- Nội dung: {x.TriThuc.NoiDungTriThuc}"
 ));

            var prompt = $@"
Bạn là trợ lý AI tư vấn khách hàng cho spa.

Nhiệm vụ:
- Trả lời khách hàng bằng tiếng Việt.
- Chỉ sử dụng thông tin trong phần TRI THỨC LIÊN QUAN.
- Không bịa thông tin ngoài dữ liệu được cung cấp.
- Nếu thông tin chưa đủ, hãy nói rõ rằng cần nhân viên tư vấn hỗ trợ thêm.
- Giọng văn lịch sự, thân thiện, chuyên nghiệp.
- Trả lời ngắn gọn, dễ hiểu.
- Nếu phù hợp, có thể gợi ý khách đặt lịch.

CÂU HỎI KHÁCH HÀNG:
{cauHoi}

TRI THỨC LIÊN QUAN:
{noiDungTriThuc}
";

            var cauTraLoiAI = await GoiGeminiAsync(prompt);

            return cauTraLoiAI;
        }

        private int TinhDiemPhuHop(string cauHoiLower, TriThuc triThuc)
        {
            var diem = 0;

            if (!string.IsNullOrWhiteSpace(triThuc.TenTriThuc)
                && cauHoiLower.Contains(triThuc.TenTriThuc.ToLower()))
            {
                diem += 5;
            }

            if (!string.IsNullOrWhiteSpace(triThuc.TuKhoa))
            {
                var tuKhoaList = triThuc.TuKhoa
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim().ToLower())
                    .ToList();

                foreach (var tuKhoa in tuKhoaList)
                {
                    if (cauHoiLower.Contains(tuKhoa))
                    {
                        diem += 4;
                    }
                }
            }

            var noiDungWords = triThuc.NoiDungTriThuc
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();

            foreach (var word in noiDungWords)
            {
                if (word.Length >= 4 && cauHoiLower.Contains(word))
                {
                    diem += 1;
                }
            }

            return diem;
        }
        private bool CanChuyenNhanVien(string cauHoiLower)
        {
            var tuKhoaChuyenNhanVien = new List<string>
            {
                "gặp nhân viên",
                "gặp người thật",
                "tư vấn viên",
                "nhân viên tư vấn",
                "khiếu nại",
                "hoàn tiền",
                "bồi thường",
                "không hài lòng",
                "nói chuyện với nhân viên",
                "cần người tư vấn",
                "gặp tư vấn"
            };

            return tuKhoaChuyenNhanVien.Any(x => cauHoiLower.Contains(x));
        }

        public async Task<ChatbotViewModel?> LayHoiThoaiDangMoCuaKhachAsync(int? idKhachHang)
        {
            if (idKhachHang == null)
            {
                return null;
            }

            var hoithoai = await _context.HoiThoaiAIs
                .Include(x => x.TinNhanAIs)
                .Where(x => x.IdKhachHang == idKhachHang && x.TrangThaiHoiThoai != "DaKetThuc")
                .OrderByDescending(x => x.ThoiGianBatDau)
                .FirstOrDefaultAsync();

            if (hoithoai == null)
            {
                return null;
            }

            return new ChatbotViewModel
            {
                IdHoiThoaiAI = hoithoai.IdHoiThoaiAI,
                TinNhans = hoithoai.TinNhanAIs
                    .OrderBy(x => x.thoiGianGui)
                    .Select(x => new TinNhanChatViewModel
                    {
                        VaiTro = x.VaiTro,
                        NoiDung = x.NoiDung,
                        ThoiGianGui = x.thoiGianGui
                    })
                    .ToList()
            };
        }
        private async Task<string> GoiGeminiAsync(string prompt)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            var model = _configuration["Gemini:Model"] ?? "gemini-2.5-flash";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return "Chưa cấu hình Gemini API Key.";
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
                return $"Lỗi gọi Gemini API: {response.StatusCode}\n{responseText}";
            }

            using var doc = JsonDocument.Parse(responseText);

            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? "Gemini không trả về nội dung.";
        }


    }
}
