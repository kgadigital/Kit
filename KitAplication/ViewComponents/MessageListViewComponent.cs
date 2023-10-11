using KitAplication.Interface;
using KitAplication.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KitAplication.ViewComponents
{
    public class MessageListViewComponent :ViewComponent
    {
        private readonly IMessageService _messageService;
        private readonly ISystemService _systemService;

        public MessageListViewComponent(IMessageService messageService, ISystemService systemService)
        {
            _messageService = messageService;
            _systemService = systemService;
        }

        public async Task <IViewComponentResult> InvokeAsync(int id)
        {
            var view = new ListMessageDisplay();
            var system = await _systemService.GetSystemByIdAsync(id);
            if (system != null)
            {
                view.SystemModel = system;
                var messages = await _messageService.GetMessagesAsync(view.SystemModel.Id);
                if (messages != null)
                {
                    view.Message = messages;
                }
            }
            
            

            return View(view);
        }
    }
}
