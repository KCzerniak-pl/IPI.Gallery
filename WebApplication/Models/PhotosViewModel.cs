namespace GalleryWebApplication.Models
{
    public class PhotosViewModel
    {
        // Właściwość przechowująca wszystkie informacje (o zdjęciach) pobrane z bazy danych.
        public PhotoViewModel[] Photos { get; set; }
    }
}
