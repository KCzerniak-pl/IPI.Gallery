using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleryWebApi.Models
{
    public class UpdatePhotoDto
    {
		[Required]
		public Guid PhotoID { get; set; }

		[Required]
		public Guid UserID { get; set; }

		[Required(AllowEmptyStrings = true)]
		[StringLength(25)]
		public string Title { get; set; }

		[Required(AllowEmptyStrings = true)]
		public string Descripton { get; set; }

		[Required]
		public List<string> Categories { get; set; }

		[Required]
		public bool Private { get; set; }
	}
}
