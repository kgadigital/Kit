using KitAplication.Models;

namespace KitAplication.Interface
{
    public interface IChatAIService
    {
        Task<string> GetResponsFromChatAI(string userinput, SystemModel systemModel,ChatSettingsModel chatSettingsModel);
    }
}
