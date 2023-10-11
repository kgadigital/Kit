using KitAplication.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitAplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IConfiguration config, ILogger<AccountController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        ///  Handles user login functionality. If the provided password matches the admin password stored in the configuration, 
        ///  creates a ClaimsPrincipal object indicating that the user is authenticated and redirects to the specified returnUrl or the default URL. 
        ///  If the password is incorrect, logs a warning message with the current date and the IP address of the request and adds a model error indicating that the password is incorrect.
        /// </summary>
        ///  <param name="model">A LoginModel object containing a password.</param>
        /// <param name="returnUrl">A string representing the URL to redirect to after successful login.</param>
        /// <returns>An asynchronous Task returning an IActionResult object</returns>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            var loginModel = new LoginModel();
            return View(loginModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Password")] LoginModel model, string? returnUrl = null)
        {
            //ViewData["returnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var adminPassword = _config["MySecretValues:Password"];
                if (model.Password == adminPassword)
                {
                    var claims = new List<Claim>
                        {
                            new Claim("IsAuthenticated", "true")
                        };


                    var identity = new ClaimsIdentity(claims, "login");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);
                    return RedirectToLocal(returnUrl);

                }
                else
                {
                    var address = Request.HttpContext.Connection.RemoteIpAddress;
                    _logger.LogWarning("Attempt to login faild. Date: {0}.IP Adress:{1}",DateTime.Now, address);
                    ModelState.AddModelError("Password", "Lösenord fel");
                }
            }

            ModelState.AddModelError("Password", "Vänligen fyll i lösenord");
            ViewData["returnUrl"] = returnUrl;
            return View(model);

        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(AdminController.Index), "Admin");
            }
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
