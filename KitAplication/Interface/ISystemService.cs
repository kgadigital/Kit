using KitAplication.Data;
using KitAplication.Models;

namespace KitAplication.Interface
{
    public interface ISystemService
    {
        Task<SystemModel> CreateSystemAsync(SystemModel system);
        Task <List<SystemModel>> GetAllAsync();
        Task<SystemModel> GetSystemByIdAsync(int id);
        Task<SystemModel> UpdateSystemAsync(SystemModel system);
        Task<SystemModel> GetActiveSystem();
        Task<bool> ActivateSystem(int id);
        Task<bool> DeactivateSystem(int id);
        Task<bool> DeleteSystem(int id);
        Task<bool> DeleteSystemAndMessagesAsync(int id);
    }
}
