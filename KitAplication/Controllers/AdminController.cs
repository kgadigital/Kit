using KitAplication.Interface;
using KitAplication.Models;
using KitAplication.Models.Enums;
using KitAplication.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Threading;
using static KitAplication.Services.ChatAIService;

namespace KitAplication.Controllers
{
     [Authorize]
    public class AdminController : Controller
    {
        private readonly ISystemService _systemService;
        private readonly IMessageService _messageService;
        private readonly IChatAIService _chatService;
        private readonly IChatSettingsService _chatSettingsService;

        public AdminController(IChatAIService chatService, IMessageService messageService, ISystemService systemService, IChatSettingsService chatSettingsService)
        {
            _chatService = chatService;
            _messageService = messageService;
            _systemService = systemService;
            _chatSettingsService = chatSettingsService;
        }

        /// <summary>
        /// This method handles the main index page for the application, it takes optional route values for system and message Ids.
        /// It sets the view model with data to populate the dropdown list and the different forms in the view.
        /// </summary>
        /// <param name="SelectSystem">An optional parameter that identifies which system is currently selected in the dropdown list.</param>
        /// <param name="updateMessageId">An optional parameter that identifies which message is currently selected for update. If not set, an empty form for creating a new message is displayed.</param>
        /// <returns>The view for the main index page populated with the necessary data for dropdown list and different forms</returns>
        public async Task<IActionResult> Index(int SelectSystem=0, int updateMessageId = 0)
        {
            /* Tempdata from actions to send to the view and display errors and message*/
            if (TempData.ContainsKey("Message:ErrorMessage"))
                ViewBag.MessageErrorMessage = TempData["Message:ErrorMessage"];
            if (TempData.ContainsKey("System:ErrorMessage"))
                ViewBag.SystemErrorMessage = TempData["System:ErrorMessage"];
            if (TempData.ContainsKey("System:Message"))
                ViewBag.SystemMessage = TempData["System:Message"];
            if (TempData.ContainsKey("Message:Message"))
                ViewBag.MessageMessage = TempData["Message:Message"];

            var view = new IndexViewModel();
            ViewBag.DropDownListSystems = await GetSystemDropdownItems(SelectSystem);

            if (SelectSystem == 0) //choise in dropdownlist: None
            {
                ViewBag.choice = 0;
                return View(view);
            }
            else if (SelectSystem == -1)//choise in dropdownlist: create new system
            {
                ViewBag.choice = -1;
                view.SystemModel = new SystemModel();
                return View(view);
            }
            else //choise in dropdownlist: selected a system
            {
                
    
                var systemEntity = await _systemService.GetSystemByIdAsync(SelectSystem); //Get the selected system
                if (systemEntity == null)
                {
                    ViewBag.choice = 0;
                    return View(view);
                }

                ViewBag.choice = 1;
                ViewBag.Roles = GetRoleList();

                view.RouteValueSelectSystem = SelectSystem; //return the selectSystem value to the view and to the partials, so they can route back the selectedSystem that are shousen in the dropdownlist of systems.
                view.SystemActive = new SystemActiveModel { SystemId=systemEntity.Id, IsActive = systemEntity.IsActive }; 
                view.SystemModel = new SystemModel() { Id = systemEntity.Id, SystemContent = systemEntity.SystemContent, SystemName = systemEntity.SystemName, Model = systemEntity.Model, Prefix = systemEntity.Prefix }; //fill the forms with selected systems content


                if (updateMessageId == 0)
                {
                    view.MessageModel = new MessageModel() { SystemId = systemEntity.Id };
                    ViewBag.MessageExist = false;
                } else 
                {
                    var message = await GetMessageById(systemEntity.Id, updateMessageId);

                    if (message == null)
                    {
                        view.MessageModel = new MessageModel() { SystemId = systemEntity.Id };
                        ViewBag.MessageExist = false;
                    }
                    else
                    {
                        view.MessageModel = new MessageModel() { Id = message.Id, SystemId = message.SystemId, Content = message.Content, RoleName = message.RoleName };
                        ViewBag.MessageExist = true;
                    }     
                }
                return View(view);
            }
        }
        private async Task<MessageModel> GetMessageById(int systemId, int messageId)
        {
            var systemMessages = await _messageService.GetMessagesAsync(systemId);

            return systemMessages.FirstOrDefault(x => x.Id == messageId);
        }
        /// <summary>
        ///  Asynchronously gets a list of SelectListItems containing system dropdown options with the given selected system ID.
        /// </summary>
        /// <param name="selectedSystemId">The ID of the selected system.</param>
        /// <returns> The task result contains a list of SelectListItems</returns>
        private async Task<List<SelectListItem>> GetSystemDropdownItems(int selectedSystemId)
        {
            var items = new List<SelectListItem> { new SelectListItem { Text = "Välj System", Value = "", Selected = true, Disabled = true } };
            var systems = await _systemService.GetAllAsync();

            foreach (var system in systems)
            {
                var isActive = system.IsActive;
                var text = isActive ? $"{system.SystemName}    (Aktiv)" : system.SystemName;

                    items.Add(new SelectListItem { Text = text, Value = system.Id.ToString(), Selected = system.Id == selectedSystemId });
               
            }

            items.Add(new SelectListItem { Text = "Skapa nytt system", Value = "-1" });

            return items;
        }


        private static List<RoleEnums> GetRoleList()
        {
            var list = new List<RoleEnums> { RoleEnums.user, RoleEnums.assistant };
            return list;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSystem(int id,int SelectSystem)
        {
            if (id == SelectSystem)
            {
                //var selectedSystem = await _systemService.GetSystemByIdAsync(id);
                if (await _systemService.DeleteSystemAndMessagesAsync(id))
                {
                    return RedirectToAction("Index");
                }else
                    return RedirectToAction("Index", new { SelectSystem = SelectSystem });
            }
            else
            return RedirectToAction("Index", new { SelectSystem = SelectSystem });


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMessage([Bind("RoleName,Content, SystemId")] MessageModel model, int SelectSystem)
        {
            if (ModelState.IsValid)
            {
                await _messageService.CreateMessageAsync(model);
                TempData["Message:Message"] = "Användarmeddelande är skapad";
                return RedirectToAction("Index", new { SelectSystem = SelectSystem });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Fyll i fältet om du vill skapa ny roll");
                TempData["Message:ErrorMessage"] = "Fyll i fältet om du vill skapa ny roll";
                return RedirectToAction("Index", new { SelectSystem = SelectSystem });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSystem([Bind(include:"SystemContent, SystemName, Model,Prefix")] SystemModel form, int SelectSystem)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(form.Prefix))
                {
                    form.Prefix = "";
                } 

                var createdsystem= await _systemService.CreateSystemAsync(form);


                if (createdsystem != null)
                {
                    TempData["System:Message"] = "System skapat";
                    SelectSystem = createdsystem.Id;
                }

                return RedirectToAction("Index", new { SelectSystem = SelectSystem });
            }
            else
            {
                SelectSystem = -1;
                ModelState.AddModelError(string.Empty, "Fyll i fältet om du vill skapa nytt system");
                TempData["System:ErrorMessage"] = "Fyll i fältet om du vill skapa nytt system";
                return RedirectToAction("Index", new { SelectSystem = SelectSystem });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSystem([Bind("SystemContent, Id, Model, SystemName,Prefix")] SystemModel form,int SelectSystem)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(form.Prefix))
                {
                    form.Prefix = "";
                }
                await _systemService.UpdateSystemAsync(form);
                TempData["System:Message"] = "System updaterad";
                return RedirectToAction("Index", new { SelectSystem = SelectSystem });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Fyll i alla fält");
                TempData["System:ErrorMessage"] = "Fyll i alla fält";
                return RedirectToAction("Index", new { SelectSystem });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMessage([Bind("Id, RoleName,Content, SystemId")] MessageModel model, int SelectSystem)
        {
            if (ModelState.IsValid)
            {
                await _messageService.UpdateMessageAsync(model);
                    TempData["Message:Message"] = "Användarmeddelande updaterad";
                    return RedirectToAction("Index", new { SelectSystem = SelectSystem });
            }
            ModelState.AddModelError(string.Empty, "Fyll i alla fält");
            TempData["Message:ErrorMassage"] = "Fyll i alla fält";
            return RedirectToAction("Index", new { SelectSystem = SelectSystem });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeliteMessage(int Id, int SelectSystem)
        {
            await _messageService.DeleteMessageAsync(Id);
           return RedirectToAction("Index", new { SelectSystem = SelectSystem });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateSystem([Bind( include:"IsActive")] int SelectSystem,SystemActiveModel form)
        {
            if (ModelState.IsValid)
            {
                if (form.IsActive == true)
                {
                    await _systemService.ActivateSystem(form.SystemId);
                }
                else
                {
                    await _systemService.DeactivateSystem(form.SystemId);
                }
                return RedirectToAction("Index", new { SelectSystem = SelectSystem });
            }
            return RedirectToAction("Index", new { SelectSystem = SelectSystem });
        }
        public IActionResult CreateRole()
        {
            var role = new ChatPrompt();
            return PartialView("_createPartial", role);
        }
    }
}
