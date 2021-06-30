using GalleryWebApplication.Models;
using System.Threading.Tasks;

namespace GalleryWebApplication.Services
{
    public interface IAccountService
    {
        Task<AuthResponseDto> LoginAsync(LoginViewModel existUser);
        Task<AuthResponseDto> RegistrationAsync(RegistrationViewModel newUser);
    }
}