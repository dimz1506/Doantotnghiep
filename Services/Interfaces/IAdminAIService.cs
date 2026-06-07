namespace Doantotnghiep.Services.Interfaces
{
    public interface IAdminAIService
    {
        Task<string> TraLoiAdminAsync(string cauHoi);
    }
}
