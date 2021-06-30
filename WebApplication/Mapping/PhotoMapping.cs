using GalleryWebApplication.Helpers;
using GalleryWebApplication.Models;
using GalleryWebApplication.Services;

namespace GalleryWebApplication.Mapping
{
    public class PhotoMapping
    {
        // Przemapowanie DTO na obiekt używany przez klienta.
        internal static PhotoViewModel GetPhotoFromDto(PhotoDto dto)
        {
            PhotoViewModel returnValue = new PhotoViewModel();

            returnValue.PhotoID = dto.PhotoID;
            returnValue.UserID = dto.UserID;
            returnValue.PhotoPath = dto.PhotoPath;
            returnValue.Title = dto.Title;
            returnValue.Descripton = dto.Descripton;
            returnValue.Categories = dto.Categories;
            returnValue.Size = dto.Size;
            returnValue.Resolution = dto.Resolution;
            returnValue.Private = dto.Private;
            returnValue.DateTimeModify = dto.DateTimeModify.ToString("dd/MM/yyyy HH:mm:ss"); ;

            return returnValue;
        }



        // Przemapowanie obiektu używanego przez klienta na DTO.
        internal static UploadPhotoDto PostPhotoToDto(PhotoViewModel newPhoto)
        {
            UploadPhotoDto returnValue = new UploadPhotoDto();

            returnValue.UserID = newPhoto.UserID;
            returnValue.PhotoFile = FileHelper.IFormFileToByte(newPhoto.PhotoFile);
            returnValue.PhotoFileName = newPhoto.PhotoFile.FileName;
            returnValue.Title = newPhoto.Title ?? string.Empty;
            returnValue.Descripton = newPhoto.Descripton ?? string.Empty;
            returnValue.Categories = newPhoto.Categories;
            returnValue.Private = newPhoto.Private;

            return returnValue;
        }



        // Przemapowanie obiektu używanego przez klienta na DTO.
        internal static UpdatePhotoDto PutPhotoToDto(PhotoViewModel editPhoto)
        {
            UpdatePhotoDto returnValue = new UpdatePhotoDto();

            returnValue.PhotoID = editPhoto.PhotoID;
            returnValue.UserID = editPhoto.UserID;
            returnValue.Title = editPhoto.Title ?? string.Empty;
            returnValue.Descripton = editPhoto.Descripton ?? string.Empty;
            returnValue.Categories = editPhoto.Categories;
            returnValue.Private = editPhoto.Private;

            return returnValue;
        }
    }
}