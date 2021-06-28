using GalleryWebApi.Models;
using System;

namespace GalleryWebApi.Mapping
{
    public class UserMapping
    {
        // Przemapowanie danych z DTO na Database.Entities.GalleryUser.
        internal static Database.Entities.GalleryUser RegisterUserToGalleryUser(UserRegistrationRequestDto user)
        {
            Database.Entities.GalleryUser returnValue = new Database.Entities.GalleryUser();

            returnValue.UserName = user.Email;
            returnValue.FirstName = user.FirstName;
            returnValue.LastName = user.LastName;
            returnValue.Email = user.Email;
            returnValue.DateTimeCreated = DateTime.Now;

            return returnValue;
        }
    }
}
