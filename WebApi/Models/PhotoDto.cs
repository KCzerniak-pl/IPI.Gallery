using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryWebApi.Models
{
    public class PhotoDto
    {
		[Required]
		public Guid PhotoID { get; set; }

		[Required]
		public Guid UserID { get; set; }

		[Required]
		public string PhotoPath { get; set; }

		[Required(AllowEmptyStrings = true)]
		public string Title { get; set; }

		[Required(AllowEmptyStrings = true)]
		public string Descripton { get; set; }

		[Required]
		public List<string> Categories { get; set; }

		[Required]
		public string Size { get; set; }

		[Required]
		public string Resolution { get; set; }

		[Required]
		public bool Private { get; set; }

		[Required]
		public DateTimeOffset DateTimeModify { get; set; }
    }
}
