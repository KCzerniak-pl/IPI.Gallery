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

        // Dependency Injection (Wstrzykiwanie zależności) przez konstruktor.
        // Zostaje wstrzyknięta usługa z PhotosService. Dzięki temu kontroler może korzystać z połączenia przez WebApi i nie musi sam go tworzyć.
        // Do prawidłowego działania wymagana jest konfiguracja Inverse of Control w pliku Startup.cs.
        public GalleryController(IPhotosService photosService)
        {
            _photosService = photosService;
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
            // Pobranie userID.
            Guid userID = Guid.Parse("46ED045F-BE63-495A-BEF6-F04B17BA3651");

            // Pobranie informacji o zdjęciach dla wybranego użytkownika (z wykorzystaniem usługi z PhotosService).
            PhotoViewModel[] photos = await _photosService.GetPhotosForUserAsync(userID);

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
            // Pobranie userID.
            Guid userID = Guid.Parse("46ED045F-BE63-495A-BEF6-F04B17BA3651");

            // Pobranie informacji o zdjęciach dla wybranego użytkownika (z wykorzystaniem usługi z PhotosService).
            PhotoViewModel photo = await _photosService.GetPhotoForUserAsync(photoID, userID);

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
            if (ModelState.IsValid)
            {
                // Pobranie userID.
                photo.UserID = Guid.Parse("46ED045F-BE63-495A-BEF6-F04B17BA3651");

                // Odwrócenie wartości publiczne/prywatne pobranej z formularza.
                photo.Private = !photo.Private;

                // Dodanie nowego lub edycja już istniejącego zdjęcia (z wykorzystaniem usługi z PhotosService).
                if (action == "save") { await _photosService.AddPhotoAsync(photo); }
                else if (action == "edit") { await _photosService.EditPhotoAsync(photo); }
            }

            return RedirectToAction(nameof(PrivatePhotos));
        }



        [HttpGet]
        public async Task<IActionResult> DeletePhoto(Guid photoID)
        {
            // Pobranie userID.
            Guid userID = Guid.Parse("46ED045F-BE63-495A-BEF6-F04B17BA3651");

            // Usunięcie informacji o wybranym zdjęciu (z wykorzystaniem usługi z PhotosService).
            await _photosService.DeletePhotoAsync(photoID, userID);

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
    }
}
