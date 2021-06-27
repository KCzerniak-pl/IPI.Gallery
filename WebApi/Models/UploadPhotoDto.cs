using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryWebApi.Models
{
    public class UploadPhotoDto
    {
		[Required]
		public Guid UserID { get; set; }

		[Required]
		public byte[] PhotoFile { get; set; }

		[Required]
		public string PhotoFileName { get; set; }

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
