using KitAplication.Data;
using KitAplication.Interface;
using KitAplication.Models;

namespace KitAplication.Services
{
    public class ChatSettingService : IChatSettingsService
    {
        private readonly IRepository<ChatSettings> _chatSettingsRepository;

        public ChatSettingService(IRepository<ChatSettings> chatSettingsRepository)
        {
            _chatSettingsRepository = chatSettingsRepository;
        }
        /// <summary>
        /// Retrieves the chat settings asynchronously from the repository and returns a ChatSettingsModel object if found, otherwise returns null.
        /// </summary>
        /// <returns>An asynchronous operation that represents the ChatSettingsModel object if found, otherwise null</returns>
        public async Task<ChatSettingsModel> GetChatSettingsAsync()
        {
            var chatSettings = await  _chatSettingsRepository.GetAllAsync();
            var firstSettings = chatSettings.FirstOrDefault();
            if (firstSettings == null)
            {
                return null;
            }
            else
            {
                return new ChatSettingsModel
                {
                    Id = firstSettings.Id,
                    RequestFailMessage = firstSettings.RequestFailMessage,
                    IsActiveMessage = firstSettings.IsActiveMessage,
                    IsNotActiveMessage = firstSettings.IsNotActiveMessage,
                };
            }
        }

        /// <summary>
        /// Updates an existing ChatSettingsModel with the provided properties
        /// </summary>
        /// <param name="model">The ChatSettingsModel object containing the updated properties.</param>
        /// <returns>The updated ChatSettingsModel object</returns>
        public async Task<ChatSettingsModel> UpdateChatSettingsAsync(ChatSettingsModel model)
        {
            var chatesettings = await _chatSettingsRepository.GetByIdAsync(model.Id);

            if (chatesettings != null)
            {
                chatesettings.RequestFailMessage = model.RequestFailMessage;
                chatesettings.IsActiveMessage = model.IsActiveMessage;
                chatesettings.IsNotActiveMessage=model.IsNotActiveMessage;


                await _chatSettingsRepository.UpdateAsync(chatesettings);
            }
            return model;
        }
        /// <summary>
        /// Adds the default chat settings asynchronously to the repository and returns a ChatSettingsModel object with the newly generated Id
        /// </summary>
        /// <returns>An asynchronous operation that represents the newly added ChatSettingsModel object</returns>
        public async Task<ChatSettingsModel> AddDefaultSettings()
        {
            var entity = new ChatSettings();
            var settings = await _chatSettingsRepository.AddAsync(entity);
            return new ChatSettingsModel { Id = settings.Id, IsActiveMessage = settings.IsActiveMessage, IsNotActiveMessage = settings.IsNotActiveMessage, RequestFailMessage = settings.RequestFailMessage};
        }
    }
}
