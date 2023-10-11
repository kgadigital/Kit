using KitAplication.Data;
using KitAplication.Interface;
using KitAplication.Models;

namespace KitAplication.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<MessageEntity> _messageRepository;

        public MessageService(IRepository<MessageEntity> messageRepository)
        {
            _messageRepository = messageRepository;
        }
        /// <summary>
        /// Creates a new MessageEntity object asynchronously in the _messageRepository and returns its ID
        /// </summary>
        /// <param name="messageModel">The MessageModel object to create the MessageEntity from</param>
        /// <returns>A Task containing the ID of the created MessageEntity object</returns>
        public async Task<int> CreateMessageAsync(MessageModel messageModel)
        {
            var messageEntity = new MessageEntity
            {
                RoleName = messageModel.RoleName,
                Content = messageModel.Content,
                SystemId = messageModel.SystemId
            };
            await _messageRepository.AddAsync(messageEntity);
            return messageEntity.Id;
        }
        /// <summary>
        /// Deletes a MessageModel object asynchronously from the _messageRepository by its ID if it exists
        /// </summary>
        /// <param name="messageId">The ID of the MessageModel object to delete</param>
        /// <returns>A Task</returns>
        public async Task DeleteMessageAsync(int messageId)
        {
            var messageEntity = await _messageRepository.GetByIdAsync(messageId);
            if (messageEntity != null)
            {
                await _messageRepository.DeleteAsync(messageEntity);
            }  
        }
        /// <summary>
        /// Gets a list of MessageModel objects asynchronously by SystemId from the _messageRepository
        /// </summary>
        /// <param name="systemId">The ID of the system to retrieve messages for</param>
        /// <returns>A Task containing a list of MessageModel objects</returns>
        public async Task<List<MessageModel>> GetMessagesAsync(int systemId)
        {
            var messages = await _messageRepository.GetAsync(m => m.SystemId == systemId);
            return messages.Select(m => new MessageModel
            {
                Id = m.Id,
                RoleName = m.RoleName,
                Content = m.Content,
                SystemId= m.SystemId
                
            }).ToList();
        }

        /// <summary>
        /// Updates a MessageModel object asynchronously in the _messageRepository if it exists
        /// </summary>
        /// <param name="messageModel">The MessageModel object to update</param>
        /// <returns>A Task</returns>
        public async Task UpdateMessageAsync(MessageModel messageModel)
        {
            var messageEntity = await _messageRepository.GetByIdAsync(messageModel.Id);
            if (messageEntity != null)
            {
                messageEntity.RoleName = messageModel.RoleName;
                messageEntity.Content = messageModel.Content;

                await _messageRepository.UpdateAsync(messageEntity);
            } 
        }
    }
}
