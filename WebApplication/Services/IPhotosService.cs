using GalleryWebApplication.Models;
using System;
using System.Threading.Tasks;

namespace GalleryWebApplication.Services
{
    public interface IPhotosService
    {
        Task<PhotoViewModel[]> GetPhotosPublicAsync();
        Task<PhotoViewModel[]> GetPhotosForUserAsync(Guid userID, string jwt);
        Task<PhotoViewModel> GetPhotoForUserAsync(Guid photoID, Guid userID, string jwt);
        Task AddPhotoAsync(PhotoViewModel newPhoto, string jwt);
        Task EditPhotoAsync(PhotoViewModel photo, string jwt);
        Task DeletePhotoAsync(Guid photoID, Guid userID, string jwt);
        Task<GetCategoriesDto> GetCategoriesAsync();
    }
}