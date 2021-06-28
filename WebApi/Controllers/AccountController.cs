using GalleryWebApi.Mapping;
using GalleryWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GalleryWebApi.Controllers
{
    [Route("apiaccount")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        protected readonly UserManager<Database.Entities.GalleryUser> _userManager;
        protected readonly IConfiguration _configuration;

        public AccountController(UserManager<Database.Entities.GalleryUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // POST: apiaccount/<ValuesController>
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] UserLoginRequestDto user)
        {
            if (ModelState.IsValid)
            {
                // Pobranie użytkownika o podanym adresie e-mail.
                var existingUser = await GetUserByEmail(user.Email);

                if (existingUser != null)
                {
                    // Sprawdzenie czy podane hasło pasuje do użytkownika o podanym adresie e-mail.
                    var isPasswordCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                    if (isPasswordCorrect)
                    {
                        // Wygenerowanie JWT.
                        var jwt = GenerateJwt(existingUser);

                        return StatusCode(StatusCodes.Status200OK, new AuthResponseDto { Success = true, Token = jwt });
                    }
                }

                return StatusCode(StatusCodes.Status400BadRequest, new AuthResponseDto { Success = false, Errors = new List<string> { "Błędny login lub hasło" } });
            }

            return StatusCode(StatusCodes.Status400BadRequest, new AuthResponseDto { Success = false, Errors = new List<string> { "Nieprawidłowe dane" } });
        }



        // POST: apiaccount/<ValuesController>
        [HttpPost("Registration")]
        public async Task<ActionResult<AuthResponseDto>> Registration([FromBody] UserRegistrationRequestDto user)
        {
            if (ModelState.IsValid)
            {
                // Pobranie użytkownika o podanym adresie e-mail.
                var existingUser = await GetUserByEmail(user.Email);

                if (existingUser == null)
                {
                    // Przemapowanie DTO na obiekt encji bazy danych.
                    var newUser = UserMapping.RegisterUserToGalleryUser(user);

                    // Utworzenie nowego używkonika
                    var isCreated = await _userManager.CreateAsync(newUser, user.Password);

                    if (isCreated.Succeeded)
                    {
                        // Wygenerowanie JWT.
                        var jwt = GenerateJwt(newUser);

                        return StatusCode(StatusCodes.Status200OK, new AuthResponseDto { Success = true, Token = jwt });
                    }

                    return StatusCode(StatusCodes.Status400BadRequest, new AuthResponseDto { Success = false, Errors = isCreated.Errors.Select(x => x.Description).ToList() });
                }

                return StatusCode(StatusCodes.Status400BadRequest, new AuthResponseDto { Success = false, Errors = new List<string> { "Konto o podanym adresie e-mail już istnieje" } });
            }

            return StatusCode(StatusCodes.Status400BadRequest, new AuthResponseDto { Success = false, Errors = new List<string> { "Nieprawidłowe dane" } });
        }



        [HttpGet]
        protected async Task<Database.Entities.GalleryUser> GetUserByEmail(string email)
        {
            // Pobranie użytkownika o podanym adresie e-mail.
            return await _userManager.FindByEmailAsync(email);
        }



        protected string GenerateJwt(Database.Entities.GalleryUser user)
        {
            // Klucz pobrany z pliku appsettings.json.
            var secret = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);

            // Konfiguracja tokena JWT.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            // Utworzenie tokena JWT.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
