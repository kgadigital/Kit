using KitAplication.Interface;
using KitAplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace KitAplication.Controllers
{
    [Authorize]
    public class ChatSettingController : Controller
    {
        private readonly IChatSettingsService _chatSettingsService;

        public ChatSettingController(IChatSettingsService chatSettingsService)
        {
            _chatSettingsService = chatSettingsService;
        }
        public async Task <IActionResult> Index()
        {
            var chatSettingModel = await _chatSettingsService.GetChatSettingsAsync();
            if (chatSettingModel == null)
            {
                return NotFound();
            }

            return View(chatSettingModel);
        }
        public async Task <IActionResult> Settings()
        {
            var chatSettingModel = await _chatSettingsService.GetChatSettingsAsync();
            if (chatSettingModel == null) {
            return NotFound();
            }

            return View(chatSettingModel);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings([Bind("Id,IsActiveMessage,IsNotActiveMessage,RequestFailMessage")] ChatSettingsModel model)
        {
            
            if (ModelState.IsValid)
            {
                await _chatSettingsService.UpdateChatSettingsAsync(model);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Alla fält måste vara ifyllda");
                    return View(model);
            }
        }
        private bool IsValidString(string value)
        {
            return !string.IsNullOrEmpty(value) && value.Trim().Length >= 5;
        }
    }
}
