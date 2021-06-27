using System.Collections.Generic;

namespace GalleryWebApi.Helpers
{
    public class CategoriesHelper
    {
        public static List<string> stringToList(string data)
        {
            string[] arr = data.Split(";");
            return new List<string>(arr); 
        }

        public static string ListToString(List<string> data)
        {
            return string.Join(";", data);
        }
    }
}