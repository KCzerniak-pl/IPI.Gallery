using Database;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using GalleryWebApi.Models;
using GalleryWebApi.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using GalleryWebApi.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace GalleryWebApi.Controllers
{
    [Route("apiphotos")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PhotosController : ControllerBase
    {
        private readonly GalleryDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostEnvironment;


        public PhotosController(GalleryDbContext dbContext, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }



        //GET: apiphotos/<ValuesController>/<UserID>
        [HttpGet("{userID}")]
        public async Task<ActionResult<IEnumerable<PhotoDto>>> PhotosForUser([FromRoute] Guid userID)
        {
            if (_dbContext.Database.CanConnect())
            {
                // Pobranie z bazy danych informacji o wszystkich zdjęciach wybranego użytkownika i przemapowanie ich na obiekt DTO.
                IEnumerable<PhotoDto> result = await _dbContext.Photos.Where(p => p.UserID == userID).Select(p => PhotoMapping.GetPhotoToDto(p)).ToListAsync();

                if (result.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }



        // GET: apiphotos/<ValuesController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PhotoDto>>> PublicPhotos()
        {
            if (_dbContext.Database.CanConnect())
            {
                // Pobranie z bazy danych informacji o wszystkich zdjęciach ustawionych jako publiczne i przemapowanie ich na obiekt PhotoDTO.
                IEnumerable<PhotoDto> result = await _dbContext.Photos.Where(p => p.Private == false).Select(p => PhotoMapping.GetPhotoToDto(p)).ToListAsync();

                if (result.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }



        // GET: apiphotos/<ValuesController>?photoid=<PhotoID>&userid=<UserID>
        [HttpGet("PhotoForUser")]
        public async Task<ActionResult<PhotoDto>> PhotoForUser([FromQuery] Guid photoID, [FromQuery] Guid userID)
        {
            if (_dbContext.Database.CanConnect())
            {
                // Pobranie z bazy danych informacji o wybranym zdjęciu dla wybranego użytkownika i przemapowanie ich na obiekt PhotoDTO.
                PhotoDto result = await _dbContext.Photos.Where(p => p.PhotoID == photoID && p.UserID == userID).Select(p => PhotoMapping.GetPhotoToDto(p)).FirstOrDefaultAsync();

                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }



        // POST apiphotos/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> UploadPhoto([FromBody] UploadPhotoDto value)
        {
            if (_dbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    // Zapisanie zdjęcia w Azure storage (oraz dostęp do informacji o jego wielkości, rozdzielczości i ścieżce zapisu).
                    AzureFileHelper file = new AzureFileHelper(value.PhotoFile, value.PhotoFileName, value.UserID, _configuration.GetConnectionString("GalleryStorage"), _hostEnvironment.ContentRootPath);

                    // Dodanie do bazy danych informacji o nowym zdjęciu.
                    _dbContext.Photos.Add(PhotoMapping.PostPhotoFromDto(value, file));
                    await _dbContext.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status204NoContent);
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }



        // PUT apiphotos/<ValuesController>
        [HttpPut]
        public async Task<IActionResult> UpdatePhoto([FromBody] UpdatePhotoDto value)
        {
            if (_dbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    // Pobranie z bazy danych informacji o wybranym zdjęciu.
                    var result = await _dbContext.Photos.Where(p => p.PhotoID == value.PhotoID && p.UserID == value.UserID).FirstOrDefaultAsync();

                    if (result != null)
                    {
                        // Akutalizacja w bazie danych informacji o wybranym zdjęciu.
                        result = PhotoMapping.PutPhotoFromDto(result, value);
                        await _dbContext.SaveChangesAsync();

                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }



        // DELETE apiphotos/<ValuesController>?photoid=<PhotoID>&userid=<UserID>
        [HttpDelete("DeletePhoto")]
        public async Task<IActionResult> DeletePhoto([FromQuery] Guid photoID, [FromQuery] Guid userID)
        {
            if (_dbContext.Database.CanConnect())
            {
                // Pobranie z bazy danych informacji o wybranym zdjęciu.
                var result = await _dbContext.Photos.Where(p => p.PhotoID == photoID && p.UserID == userID).FirstOrDefaultAsync();

                if (result != null)
                {
                    // Usunięcie bloba.
                    await AzureFileHelper.DeleteBlob(result.PhotoPath, _configuration.GetConnectionString("GalleryStorage"));

                    // Usunięcie z bazy danych informacji o wybranym zdjęciu.
                    _dbContext.Photos.Remove(result);
                    await _dbContext.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status204NoContent);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}