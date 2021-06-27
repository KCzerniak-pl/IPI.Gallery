using GalleryWebApplication.Mapping;
using GalleryWebApplication.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GalleryWebApplication.Services
{
    public class PhotosService : IPhotosService
    {
        private readonly string _url = "http://localhost:58685";



        // Pobranie przez serwis informacji o wszystkich publicznych zdjęciach.
        public async Task<PhotoViewModel[]> GetPhotosPublicAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Korzystamy z klienta, który został wygenerowany z wykorzystaniem narzędzia NSwagStudio.
                GalleryWebApiClient apiClient = new GalleryWebApiClient(_url, httpClient);

                // Pobranie przez serwis informacji o wszystkich publicznych zdjęciach.
                var photos = await apiClient.PublicPhotosAsync();

                // Przemapowanie z DTO na obiekt używany przez klienta.
                return photos.Select(dto => PhotoMapping.GetPhotoFromDto(dto)).ToArray();
            }
        }



        // Pobranie przez serwis informacji o wszystkich zdjęciach dla wybranego użytkownika.
        public async Task<PhotoViewModel[]> GetPhotosForUserAsync(Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Korzystamy z klienta, który został wygenerowany z wykorzystaniem narzędzia NSwagStudio.
                GalleryWebApiClient apiClient = new GalleryWebApiClient(_url, httpClient);

                // Pobranie przez serwis informacji o wszystkich zdjęciach dla wybranego użytkownika.
                var photos = await apiClient.PhotosForUserAsync(userID);

                // Przemapowanie z DTO na obiekt używany przez klienta.
                return photos.Select(dto => PhotoMapping.GetPhotoFromDto(dto)).ToArray();
            }
        }



        // Pobranie przez serwis informacji o wybranym zdjęciu.
        public async Task<PhotoViewModel> GetPhotoForUserAsync(Guid photoID, Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Korzystamy z klienta, który został wygenerowany z wykorzystaniem narzędzia NSwagStudio.
                GalleryWebApiClient apiClient = new GalleryWebApiClient(_url, httpClient);

                // Pobranie informacji o zdjęciach dla wybranego użytkownika.
                var photo = await apiClient.PhotoForUserAsync(photoID, userID);

                // Przemapowanie z DTO na obiekt używany przez klienta.
                return PhotoMapping.GetPhotoFromDto(photo);
            }
        }



        // Dodanie przez serwis nowego zdjęcia.
        public async Task AddPhotoAsync(PhotoViewModel newPhoto)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Korzystamy z klienta, który został wygenerowany z wykorzystaniem narzędzia NSwagStudio.
                GalleryWebApiClient apiClient = new GalleryWebApiClient(_url, httpClient);

                // Przemapowanie z obiektu używanego przez klienta na DTO i przesłanie informacji o nowym zdjęciu.
                await apiClient.UploadPhotoAsync(PhotoMapping.PostPhotoToDto(newPhoto));
            }
        }



        // Aktualizacja przez serwis wybranego zdjęcia.
        public async Task EditPhotoAsync(PhotoViewModel editPhoto)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Korzystamy z klienta, który został wygenerowany z wykorzystaniem narzędzia NSwagStudio.
                GalleryWebApiClient apiClient = new GalleryWebApiClient(_url, httpClient);

                // Przemapowanie z obiektu używanego przez klienta na DTO i przesłanie informacji o nowym zdjęciu.
                await apiClient.UpdatePhotoAsync(PhotoMapping.PutPhotoToDto(editPhoto));
            }
        }



        // Usunięcie przez serwis wybranego zdjęcia
        public async Task DeletePhotoAsync(Guid photoID, Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Korzystamy z klienta, który został wygenerowany z wykorzystaniem narzędzia NSwagStudio.
                GalleryWebApiClient apiClient = new GalleryWebApiClient(_url, httpClient);

                // Usunięcie informacji o wybranym zdjęciu dla wybranego użytkownika.
                await apiClient.DeletePhotoAsync(photoID, userID);
            }
        }



        // Pobranie przez serwis informacji o dostępnych kategoriach.
        public async Task<GetCategoriesDto> GetCategoriesAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Korzystamy z klienta, który został wygenerowany z wykorzystaniem narzędzia NSwagStudio.
                GalleryWebApiClient apiClient = new GalleryWebApiClient(_url, httpClient);

                // Pobranie przez serwis informacji o dostępnych kategoriach.
                return await apiClient.CategoriesAsync();
            }
        }
    }
}
