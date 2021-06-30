using GalleryWebApplication.Mapping;
using GalleryWebApplication.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace GalleryWebApplication.Services
{
    public class AccountService : IAccountService
    {
        private readonly string _url = "http://localhost:58685";



        // Logowanie użytkownika przez serwis.
        public async Task<AuthResponseDto> LoginAsync(LoginViewModel existUser)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Korzystamy z klienta, który został wygenerowany z wykorzystaniem narzędzia NSwagStudio.
                GalleryWebApiClient apiClient = new GalleryWebApiClient(_url, httpClient);

                // Przemapowanie z obiektu używanego przez klienta na DTO i przesłanie informacji do zalogowania.
                return await apiClient.LoginAsync(AccountMapping.PostLoginToDto(existUser));
            }
        }



        // Rejestracja nowego użytkownika przez serwis.
        public async Task<AuthResponseDto> RegistrationAsync(RegistrationViewModel newUser)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Korzystamy z klienta, który został wygenerowany z wykorzystaniem narzędzia NSwagStudio.
                GalleryWebApiClient apiClient = new GalleryWebApiClient(_url, httpClient);

                // Przemapowanie z obiektu używanego przez klienta na DTO i przesłanie informacji do zalogowania.
                return await apiClient.RegistrationAsync(AccountMapping.PostRegistrationToDto(newUser));
            }
        }
    }
}
