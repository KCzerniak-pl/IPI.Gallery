using GalleryWebApplication.Models;
using GalleryWebApplication.Services;

namespace GalleryWebApplication.Mapping
{
    public class AccountMapping
    {
        // Przemapowanie obiektu używanego przez klienta na DTO.
        internal static UserLoginRequestDto PostLoginToDto(LoginViewModel existUser)
        {
            UserLoginRequestDto returnValue = new UserLoginRequestDto();

            returnValue.Email = existUser.Email;
            returnValue.Password = existUser.Password;

            return returnValue;
        }



        // Przemapowanie obiektu używanego przez klienta na DTO.
        internal static UserRegistrationRequestDto PostRegistrationToDto(RegistrationViewModel newUser)
        {
            UserRegistrationRequestDto returnValue = new UserRegistrationRequestDto();

            returnValue.FirstName = newUser.FirstName;
            returnValue.LastName = newUser.LastName;
            returnValue.Email = newUser.Email;
            returnValue.Password = newUser.Password;

            return returnValue;
        }
    }
}
