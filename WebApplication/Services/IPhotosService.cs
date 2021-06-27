using GalleryWebApplication.Models;
using System;
using System.Threading.Tasks;

namespace GalleryWebApplication.Services
{
    public interface IPhotosService
    {
        Task<PhotoViewModel[]> GetPhotosPublicAsync();
        Task<PhotoViewModel[]> GetPhotosForUserAsync(Guid userID);
        Task<PhotoViewModel> GetPhotoForUserAsync(Guid photoID, Guid userID);
        Task AddPhotoAsync(PhotoViewModel newPhoto);
        Task EditPhotoAsync(PhotoViewModel photo);
        Task DeletePhotoAsync(Guid photoID, Guid userID);
        Task<GetCategoriesDto> GetCategoriesAsync();
    }
}