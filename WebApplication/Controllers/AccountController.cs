using GalleryWebApplication.Helpers;
using GalleryWebApplication.Models;
using GalleryWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GalleryWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SessionHelper _sessionHelper;

        // Dependency Injection (Wstrzykiwanie zależności) przez konstruktor.
        // Zostaje wstrzyknięta usługa z AccountService. Dzięki temu kontroler może korzystać z połączenia przez WebApi i nie musi sam go tworzyć.
        // Do prawidłowego działania wymagana jest konfiguracja Inverse of Control w pliku Startup.cs.
        public AccountController(IAccountService accountService, SessionHelper sessionHelper)
        {
            _accountService = accountService;
            _sessionHelper = sessionHelper;
        }



        [HttpGet]
        public IActionResult Login([FromQuery] string returnUrl = "/gallery/privatephotos")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel existUser, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Logowanie wybranego użytkownika (z wykorzystaniem usługi z AccountService).
                var authResponse = await _accountService.LoginAsync(existUser);

                if (authResponse.Success)
                {
                    // Zapisanie informacji w sesji / cookies.
                    _sessionHelper.SetObjectAsJson(HttpContext, authResponse, existUser.RemeberMe);
                    
                    return RedirectToLocal(returnUrl);
                }

                // Przypisanie informacji o błędach do ViewBag.
                ViewBag.Errors = authResponse.Errors;
            }

            return View();
        }



        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                // Utworzenie nowego użytkownika (z wykorzystaniem usługi z AccountService).
                var authResponse = await _accountService.RegistrationAsync(newUser);

                if (authResponse.Success)
                {
                    // Zapisanie informacji w sesji.
                    _sessionHelper.SetObjectAsJson(HttpContext, authResponse);

                    return RedirectToAction(nameof(GalleryController.PrivatePhotos), "Gallery");
                }

                // Przypisanie informacji o błędach do ViewBag.
                ViewBag.Errors = authResponse.Errors;
            }

            return View();
        }



        [HttpGet]
        public IActionResult Logout()
        {
            // Usunięcie sesji / cookies.
            _sessionHelper.Logout(HttpContext);

            return RedirectToAction(nameof(GalleryController.Index), "Gallery");
        }



        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(GalleryController.Index), "Gallery");
        }
    }
}
