using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace GalleryWebApplication.Models
{
    public class PhotoViewModel
    {
        public PhotosViewModel[] Photos { get; set; }

        public Guid PhotoID { get; set; }

        public Guid UserID { get; set; }

        public string PhotoPath { get; set; }

        [Display(Name = "Tytuł:")]
        public string Title { get; set; }

        [Display(Name = "Opis:")]
        public string Descripton { get; set; }

        [Display(Name = "Kategoria:")]
        public string[] Categories { get; set; }

        public string Size { get; set; }

        public string Resolution { get; set; }

        [Display(Name = "Ustaw jako publiczne")]
        public bool Private { get; set; }

        public string DateTimeModify { get; set; }

        [Display(Name = "Zdjęcie:")]
        public IFormFile PhotoFile { get; set; }
    }
}
