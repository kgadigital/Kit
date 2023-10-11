using KitAplication.Data;
using KitAplication.Models;

namespace KitAplication.Interface
{
    public interface IChatSettingsService
    {
        Task<ChatSettingsModel> GetChatSettingsAsync();
        Task<ChatSettingsModel> UpdateChatSettingsAsync(ChatSettingsModel chatSettings);
        Task<ChatSettingsModel> AddDefaultSettings();
    }
}
