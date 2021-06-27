using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MimeTypes;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace GalleryWebApi.Helpers
{
    public class AzureFileHelper
    {
        // Ścieżka zapisu zdjęcia.
        public string PhotoPath { get; set; }

        // Wielkość zdjęcia.
        public float PhotoSize { get; set; }

        // Rozdzielczość zdjęcia.
        public string PhotoResolution { get; set; }



        public AzureFileHelper(byte[] PhotoFile, string PhotoFileName, Guid UserID, string connectionString, string contentRootPath)
        {
            // Przekonwertowanie byte[] do Image i pobranie informacji o wielkości, rozdzielczości i ścieżce do pliku tymczasowego.
            ByteToImage(PhotoFile, PhotoFileName, contentRootPath);

            // Dodanie bloba i pobranie ścieżki do zdjęcia w Azure storage.
            PhotoPath = AddBlob(UserID, connectionString).Result;
        }



        // Przekonwertowanie byte[] do Image i pobranie informacji o wielkości, rozdzielczości i ścieżce do pliku tymczasowego.
        private void ByteToImage(byte[] PhotoFile, string PhotoFileName, string contentRootPath)
        {
            using (MemoryStream ms = new MemoryStream(PhotoFile))
            {
                // Jeżeli folder dla tymczasowych zdjęć nie istnieje, to zostanie utworzony.
                string rootPath = Path.Combine(contentRootPath, "Images");
                Directory.CreateDirectory(rootPath);

                // Ścieżka do zdjęcia.
                PhotoPath = Path.Combine(rootPath, string.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(PhotoFileName)));

                // Utworzenie zdjęcia.
                Image photo = Image.FromStream(ms);

                // Zapisanie zdjęcia.
                photo.Save(PhotoPath);

                // Wielkość zdjęcia.
                PhotoSize = (float)Math.Round((float)Convert.ToInt32(ms.Length) / 1024, 2);

                // Rozdzielczość zdjęcia.
                PhotoResolution = string.Format("{0}x{1}", photo.Width.ToString(), photo.Height.ToString());
            }
        }



        // Dodanie bloba.
        private async Task<string> AddBlob(Guid UserID, string connectionString)
        {
            // Nazwa kontenera.
            string containerName = string.Format($"gallery-{UserID}").ToLower();

            // Nazwa bloba.
            string blobName = Path.GetFileName(PhotoPath);

            try
            {
                // Nawiązanie połączenia z Azure Blobs.
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // Uchwyt do kontenera o podanej nazwie.
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Utworzenie kontenera, jeżeli nie istnieje.
                await blobContainerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                // Uchwyt do bloba o podanej nazwie.
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

                // Ustawienie ContentType na podstawie rozszerzenia pliku.
                BlobHttpHeaders blobHttpHeaders = new BlobHttpHeaders();
                blobHttpHeaders.ContentType = MimeTypeMap.GetMimeType(Path.GetExtension(blobName));

                // Przesłanie bloba do kontenera.
                await blobClient.UploadAsync(PhotoPath, blobHttpHeaders);

                // Zwrócenie ścieżki do pliku w Azure storage.
                return blobClient.Uri.ToString();
            }
            finally
            {
                // Usunięcie pliku tymczasowego.
                if (File.Exists(PhotoPath)) { File.Delete(PhotoPath); }
            }
        }



        // Usunięcie bloba.
        public static async Task DeleteBlob(string photoPath, string connectionString)
        {
            // Nazwa kontenera.
            string containerName = new DirectoryInfo(photoPath).Parent.Name;

            // Nazwa bloba.
            string blobName = Path.GetFileName(photoPath);

            // Nawiązanie połączenia z Azure Blobs.
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Uchwyt do kontenera o podanej nazwie.
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Uchwyt do bloba o podanej nazwie.
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            // Jeżeli blob istnieje.
            if (await blobClient.ExistsAsync())
            {
                // Usunięcie bloba.
                await blobClient.DeleteIfExistsAsync();
            }
        }
    }
}