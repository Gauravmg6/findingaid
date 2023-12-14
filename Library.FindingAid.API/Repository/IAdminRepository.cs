using Library.FindingAid.API.Models;

namespace Library.FindingAid.API.Repository
{
    public interface IAdminRepository
    {
        Task AddAdmin(string UserId);
        Task RemoveAdmin(string userSid);
        Task<List<Admin>> GetAll();
        Task<bool> IsAdmin(string UserId);
    }
}
