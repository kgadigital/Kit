using KitAplication.Data;
using KitAplication.Interface;
using KitAplication.Models;
using KitAplication.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KitAplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISystemService _systemService;
        private readonly ILinkService _linkService;
        private readonly IChatSettingsService _chatSettingsService;
        private readonly IChatAIService _chatAIService;
        private readonly IFileHandler _fileHandler;


        public HomeController(ILogger<HomeController> logger, ISystemService systemService, ILinkService linkService, IChatSettingsService chatSettingsService, IChatAIService chatAIService, IFileHandler fileHandler)
        {
            _logger = logger;
            _systemService = systemService;
            _linkService = linkService;
            _chatSettingsService = chatSettingsService;
            _chatAIService = chatAIService;
            _fileHandler = fileHandler;
        }

        public async Task <IActionResult> Index()
        {
            //Hämta länkar
            var links = await _linkService.GetLinksAsync();
            var linkslist = new List<LinkModel>();
            foreach (var link in links)
                linkslist.Add(new LinkModel { Name = link.Name, Url = link.Url });

            //Hämta ChattSettings
            var chatsettings = await _chatSettingsService.GetChatSettingsAsync();

            //Sätt default values when no settings exist
            chatsettings ??= await _chatSettingsService.AddDefaultSettings();
           

            //Hämta aktivt system
            var ActiveSystem = await _systemService.GetActiveSystem();
            if (ActiveSystem != null)
            {
                if (ActiveSystem.IsActive)
                {
                    ViewBag.Message = chatsettings.IsActiveMessage;
                    ViewBag.IsActive = true;
                }
            }
            else
            {
                ViewBag.Message = chatsettings.IsNotActiveMessage;
                ViewBag.IsActive = false;
            }
            return View(linkslist);
        }


        //[FromBody]provide the value for the parameter from the body of the request
        [HttpPost]
        public async Task<string> GetAnswer(string questionInput)
        {
            

            //Hämta ChattSettings
            var chatsettings = await _chatSettingsService.GetChatSettingsAsync();

            //if input empty return chatsettings active message,dont make call to api
            if (questionInput == null || questionInput == "")
                return chatsettings.IsActiveMessage;

            //Get stored active system
            var activeSystem = await _systemService.GetActiveSystem();
            if (activeSystem == null)
                return chatsettings.IsNotActiveMessage;

            
            //get answer
            var returnAnswer = await _chatAIService.GetResponsFromChatAI(questionInput.ToString(), activeSystem,chatsettings);
            //Logg input and answer
            _fileHandler.LoggQAToTextFileMonthly("User input: " + questionInput, "Assistant respons: " + returnAnswer, TextLoggLevel.Information);

            return returnAnswer;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}