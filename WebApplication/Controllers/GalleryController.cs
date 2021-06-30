using GalleryWebApplication.Helpers;
using GalleryWebApplication.Models;
using GalleryWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryWebApplication.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IPhotosService _photosService;
        private readonly SessionHelper _sessionHelper;

        // Dependency Injection (Wstrzykiwanie zależności) przez konstruktor.
        // Zostaje wstrzyknięta usługa z PhotosService. Dzięki temu kontroler może korzystać z połączenia przez WebApi i nie musi sam go tworzyć.
        // Do prawidłowego działania wymagana jest konfiguracja Inverse of Control w pliku Startup.cs.
        public GalleryController(IPhotosService photosService, SessionHelper sessionHelper)
        {
            _photosService = photosService;
            _sessionHelper = sessionHelper;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Pobranie informacji o wszystkich publicznych zdjęciach.
            PhotoViewModel[] photos = await _photosService.GetPhotosPublicAsync();

            // Inicjalizacja obiektu z modelem danych i przypisanie pobranych informacji.
            PhotosViewModel photosViewModel = new PhotosViewModel() { Photos = photos };

            // Pobranie informacji o dostępnych kategoriach (z wykorzystaniem usługi z PhotosService)
            GetCategoriesDto categoriesDTO = await _photosService.GetCategoriesAsync();

            // Przypisanie informacji o dostępnych kategoriach do ViewBag.
            ViewBag.Categories = categoriesDTO.Categories;

            return View(photosViewModel.Photos.OrderByDescending(x => x.DateTimeModify));
        }



        [HttpGet]
        public async Task<IActionResult> PrivatePhotos()
        {
            // Sprawdzenie autoryzacji do strony.
            if (!CheckAuthorization())
            {
                // W przypadku braku dostępu przekierowanie do strony logowania (z parametrem powrotu do PrivatePhotos).
                return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = Url.Action(nameof(PrivatePhotos), "Gallery") });
            }

            // Pobranie JWT zapisanego w sesji.
            string jwt = _sessionHelper.GetObjectFromJson<string>(HttpContext, "JWToken");

            // Pobranie userID zapisanego w JWT.
            Guid userID = _sessionHelper.GetObjectFromJwt<Guid>(jwt, "Id");

            // Pobranie informacji o zdjęciach dla wybranego użytkownika (z wykorzystaniem usługi z PhotosService).
            PhotoViewModel[] photos = await _photosService.GetPhotosForUserAsync(userID, jwt);

            // Inicjalizacja obiektu z modelem danych i przypisanie pobranych informacji.
            PhotosViewModel galleryViewModel = new PhotosViewModel() { Photos = photos };

            // Pobranie informacji o dostępnych kategoriach (z wykorzystaniem usługi z PhotosService)
            GetCategoriesDto categoriesDTO = await _photosService.GetCategoriesAsync();

            // Przypisanie informacji o dostępnych kategoriach do ViewBag.
            ViewBag.Categories = categoriesDTO.Categories;

            return View(galleryViewModel.Photos.OrderByDescending(x => x.DateTimeModify));
        }



        [HttpGet]
        public async Task<IActionResult> AddPhoto()
        {
            // Sprawdzenie autoryzacji do strony.
            if (!CheckAuthorization())
            {
                // W przypadku braku dostępu przekierowanie do strony logowania (z parametrem powrotu do AddPhoto).
                return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = Url.Action(nameof(AddPhoto), "Gallery") });
            }

            // Pobranie informacji o dostępnych kategoriach (z wykorzystaniem usługi z PhotosService).
            GetCategoriesDto categoriesDTO = await _photosService.GetCategoriesAsync();

            // Przygotowanie danych dla listy rozijanej z kategoriami.
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            categoriesDTO.Categories.ToList().ForEach(x => selectListItem.Add(new SelectListItem { Value = x, Text = x.Replace("-", " ") }));
            ViewBag.Categories = selectListItem;

            return View();
        }



        [HttpGet]
        public async Task<IActionResult> EditPhoto(Guid photoID)
        {
            // Sprawdzenie autoryzacji do strony.
            if (!CheckAuthorization())
            {
                // W przypadku braku dostępu przekierowanie do strony logowania (z parametrem powrotu do PrivatePhotos).
                return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = Url.Action(nameof(PrivatePhotos), "Gallery") });
            }

            // Pobranie JWT zapisanego w sesji. 
            string jwt = _sessionHelper.GetObjectFromJson<string>(HttpContext, "JWToken");

            // Pobranie userID zapisanego w JWT.
            Guid userID = _sessionHelper.GetObjectFromJwt<Guid>(jwt, "Id");

            // Pobranie informacji o zdjęciach dla wybranego użytkownika (z wykorzystaniem usługi z PhotosService).
            PhotoViewModel photo = await _photosService.GetPhotoForUserAsync(photoID, userID, jwt);

            photo.Private = !photo.Private;

            // Pobranie informacji o dostępnych kategoriach (z wykorzystaniem usługi z PhotosService).
            GetCategoriesDto categoriesDTO = await _photosService.GetCategoriesAsync();

            // Przygotowanie danych dla listy rozijanej z kategoriami.
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            categoriesDTO.Categories.ToList().ForEach(x => selectListItem.Add(new SelectListItem { Value = x, Text = x.Replace("-", " ") }));
            ViewBag.Categories = selectListItem;

            return View(photo);
        }



        [HttpPost]
        [ValidateAntiForgeryToken] // Sprawdzenie tokena zabezpieczającego formularz.
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filtr wykonany w przypadku błędnego tokena zabezpieczającego formularz.
        public async Task<IActionResult> SavePhoto(PhotoViewModel photo, string action)
        {
            // Sprawdzenie autoryzacji do strony.
            if (!CheckAuthorization())
            {
                // W przypadku braku dostępu przekierowanie do strony logowania (z parametrem powrotu do PrivatePhotos).
                return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = Url.Action(nameof(PrivatePhotos), "Gallery") });
            }

            if (ModelState.IsValid)
            {
                // Pobranie JWT zapisanego w sesji. 
                string jwt = _sessionHelper.GetObjectFromJson<string>(HttpContext, "JWToken");

                // Pobranie userID zapisanego w JWT.
                photo.UserID = _sessionHelper.GetObjectFromJwt<Guid>(jwt, "Id");

                // Odwrócenie wartości publiczne/prywatne pobranej z formularza.
                photo.Private = !photo.Private;

                // Dodanie nowego lub edycja już istniejącego zdjęcia (z wykorzystaniem usługi z PhotosService).
                if (action == "save") { await _photosService.AddPhotoAsync(photo, jwt); }
                else if (action == "edit") { await _photosService.EditPhotoAsync(photo, jwt); }
            }

            return RedirectToAction(nameof(PrivatePhotos));
        }



        [HttpGet]
        public async Task<IActionResult> DeletePhoto(Guid photoID)
        {
            // Sprawdzenie autoryzacji do strony. 
            if (!CheckAuthorization())
            {
                // W przypadku braku dostępu przekierowanie do strony logowania (z parametrem powrotu do PrivatePhotos).
                return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = Url.Action(nameof(PrivatePhotos), "Gallery") });
            }

            // Pobranie JWT zapisanego w sesji. 
            string jwt = _sessionHelper.GetObjectFromJson<string>(HttpContext, "JWToken");

            // Pobranie userID zapisanego w JWT.
            Guid userID = _sessionHelper.GetObjectFromJwt<Guid>(jwt, "Id");

            // Usunięcie informacji o wybranym zdjęciu (z wykorzystaniem usługi z PhotosService).
            await _photosService.DeletePhotoAsync(photoID, userID, jwt);

            return RedirectToAction(nameof(PrivatePhotos));
        }



        [HttpGet("/aboutproject")]
        public IActionResult AboutProject()
        {
            return View();
        }



        [HttpGet("/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }



        public bool CheckAuthorization()
        {
            // Pobranie informacji zapisanych w sesji. 
            // Jeżeli w sesji nie ma klucza "Authorization" zostanie zwrócone "false".
            return _sessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization");
        }
    }
}