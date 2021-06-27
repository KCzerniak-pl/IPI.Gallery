using Microsoft.AspNetCore.Http;
using System.IO;

namespace GalleryWebApplication.Helpers
{
    public class FileHelper
    {
        // Przekonwertowanie pliku z formularza do byte[].
        public static byte[] IFormFileToByte(IFormFile fileIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Przekonwertowanie IFormFile do byte[].
                fileIn.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
