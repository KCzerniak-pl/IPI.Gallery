using GalleryWebApi.Helpers;
using GalleryWebApi.Models;
using System;

namespace GalleryWebApi.Mapping
{
    public class PhotoMapping
    {
        // Przemapowanie pobranych informacji z bazy danych na DTO.
        internal static PhotoDto GetPhotoToDto(Database.Entities.Photo photo)
        {
            PhotoDto returnValue = new PhotoDto();

            returnValue.PhotoID = photo.PhotoID;
            returnValue.UserID = photo.UserID;
            returnValue.PhotoPath = photo.PhotoPath;
            returnValue.Title = photo.Title;
            returnValue.Descripton = photo.Descripton;
            returnValue.Categories = CategoriesHelper.stringToList(photo.Categories);
            returnValue.Size = photo.Size.ToString();
            returnValue.Resolution = photo.Resolution;
            returnValue.Private = photo.Private;
            returnValue.DateTimeModify = photo.DateTimeModify;

            return returnValue;
        }



        // Przemapowanie DTO na informacje, które będą zapisane w bazie danych.
        internal static Database.Entities.Photo PostPhotoFromDto(UploadPhotoDto value, AzureFileHelper file)
        {
            Database.Entities.Photo returnValue = new Database.Entities.Photo();

            returnValue.UserID = value.UserID;
            returnValue.PhotoPath = file.PhotoPath;
            returnValue.Title = value.Title;
            returnValue.Descripton = value.Descripton;
            returnValue.Categories = CategoriesHelper.ListToString(value.Categories);
            returnValue.Size = file.PhotoSize;
            returnValue.Resolution = file.PhotoResolution;
            returnValue.Private = value.Private;
            returnValue.DateTimeModify = DateTime.Now;

            return returnValue;
        }



        // Przemapowanie DTO na informacje, które będą zapisane w bazie danych.
        internal static Database.Entities.Photo PutPhotoFromDto(Database.Entities.Photo result, UpdatePhotoDto value)
        {
            result.Title = value.Title;
            result.Descripton = value.Descripton;
            result.Categories = CategoriesHelper.ListToString(value.Categories);
            result.Private = value.Private;
            result.DateTimeModify = DateTime.Now;

            return result;
        }
    }
}
