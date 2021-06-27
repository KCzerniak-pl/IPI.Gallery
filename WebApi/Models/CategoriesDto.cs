using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleryWebApi.Models
{
    // DTO (Data Transfer Objects) - ten obiekt będzie przesyłany między usługą, a klientem.
    public class CategoriesDto
	{
		[Required]
		public List<string> Categories { get; set; }
	}
}
