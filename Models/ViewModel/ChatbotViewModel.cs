namespace Doantotnghiep.Models.ViewModel
{
    public class ChatbotViewModel
    {
        public int? IdHoiThoaiAI {  get; set; }
        public string CauHoi {  get; set; } = string.Empty;
        public List<TinNhanChatViewModel> TinNhans { get; set; } = new List<TinNhanChatViewModel>();
    }

    public class TinNhanChatViewModel
    {
        public string VaiTro { get; set; } = string.Empty;
        public string NoiDung {  get; set; } = string.Empty;
        public DateTime ThoiGianGui { get; set; }
     }
} 
