using GalleryWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GalleryWebApi.Controllers
{
    [Route("apicategories")]
    [ApiController]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        // GET: apicategories/<ValuesController>
        [HttpGet]
        public ActionResult<CategoriesDto> Categories()
        {
            List<string> categoriesList = new List<string>();
            categoriesList.Add("Ludzie");
            categoriesList.Add("Portret");
            categoriesList.Add("Zwierzęta");
            categoriesList.Add("Natura");
            categoriesList.Add("Sport");
            categoriesList.Add("Motoryzacja");
            categoriesList.Add("Nowe-technologie");
            categoriesList.Add("Hobby");
            categoriesList.Add("Moda");
            categoriesList.Add("Kulinaria");
            categoriesList.Add("Architektura");
            categoriesList.Add("Inne");

            var result =  new CategoriesDto
            {
                Categories = categoriesList
            };

            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}
