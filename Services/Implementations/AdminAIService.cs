using System.Text;
using System.Text.Json;
using Doantotnghiep.Services.Interfaces;

namespace Doantotnghiep.Services
{
    public class AdminAIService : IAdminAIService
    {
        private readonly IThongKeService _thongKeService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AdminAIService(
            IThongKeService thongKeService,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _thongKeService = thongKeService;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> TraLoiAdminAsync(string cauHoi)
        {
            var today = DateTime.Today;
            var start7Days = today.AddDays(-7);
            var year = today.Year;

            var doanhThuNgay = await _thongKeService.GetDoanhThuTheoNgayAsync(start7Days, today);
            var doanhThuThang = await _thongKeService.GetDoanhThuTheoThangAsync(year);
            var dichVuBanChay = await _thongKeService.GetDichVuBanChayAsync(null, null);
            var thongKeNhanVien = await _thongKeService.GetThongKeNhanVienAsync(null, null);

            var duLieuThongKe = new
            {
                NgayHienTai = today.ToString("dd/MM/yyyy"),
                DoanhThu7NgayGanNhat = doanhThuNgay,
                DoanhThuTheoThangTrongNam = doanhThuThang,
                DichVuBanChay = dichVuBanChay,
                ThongKeNhanVien = thongKeNhanVien
            };

            var prompt = $@"
Bạn là AI Agent hỗ trợ quản trị viên spa.

Bạn có thể hỗ trợ:
1. Phân tích doanh thu theo ngày.
2. Phân tích doanh thu theo tháng.
3. Phân tích dịch vụ bán chạy.
4. Phân tích hiệu suất nhân viên.
5. Gợi ý kinh doanh dựa trên doanh thu, dịch vụ và nhân viên.
6. Tóm tắt dữ liệu hệ thống.
7. Hỗ trợ admin ra quyết định.

Quy tắc:
- Chỉ dùng dữ liệu hệ thống cung cấp.
- Không bịa số liệu.
- Nếu thiếu dữ liệu thì nói rõ chưa đủ dữ liệu.
- Trả lời bằng tiếng Việt.
- Trả lời ngắn gọn, rõ ràng, có nhận xét và đề xuất nếu phù hợp.
- Đơn vị tiền là VNĐ.

Câu hỏi của admin:
{cauHoi}

Dữ liệu hệ thống:
{JsonSerializer.Serialize(duLieuThongKe)}
";

            return await GoiGeminiAsync(prompt);
        }

        private async Task<string> GoiGeminiAsync(string prompt)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            var model = _configuration["Gemini:Model"] ?? "gemini-2.5-flash";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return "Chưa cấu hình Gemini API Key trong appsettings.json.";
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

            var response = await _httpClient.SendAsync(request);
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