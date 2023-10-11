using KitAplication.Models;

namespace KitAplication.Interface
{
    public interface IMessageService
    {
        Task<List<MessageModel>> GetMessagesAsync(int systemId);
        Task<int> CreateMessageAsync(MessageModel messageModel);
        Task UpdateMessageAsync(MessageModel messageModel);
        Task DeleteMessageAsync(int messageId);
    }
}
