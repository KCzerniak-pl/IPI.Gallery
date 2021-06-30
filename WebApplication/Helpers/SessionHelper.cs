using GalleryWebApplication.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace GalleryWebApplication.Helpers
{
    public class SessionHelper
    {
        // Zapisanie informacji w sesji / cookies.
        public void SetObjectAsJson(HttpContext httpContext, AuthResponseDto authResponse, bool cookies = false)
        {
            // Zapisane informacji w sesji.
            string authorization = JsonConvert.SerializeObject(authResponse.Success);
            httpContext.Session.SetString("Authorization", authorization);

            string jwtoken = JsonConvert.SerializeObject(authResponse.Token);
            httpContext.Session.SetString("JWToken", jwtoken);

            string userName = JsonConvert.SerializeObject(GetObjectFromJwt<string>(authResponse.Token, JwtRegisteredClaimNames.Sub));
            httpContext.Session.SetString("UserName", userName);

            if (cookies)
            {
                // Zapisanie informacji w cookies.
                DateTimeOffset dataTimeExpires = DateTimeOffset.Now.AddHours(24);
                httpContext.Response.Cookies.Append("Authorization", authorization, new CookieOptions { Expires = dataTimeExpires });
                httpContext.Response.Cookies.Append("JWToken", jwtoken, new CookieOptions { Expires = dataTimeExpires });
                httpContext.Response.Cookies.Append("UserName", userName, new CookieOptions { Expires = dataTimeExpires });
            }
        }



        // Pobranie informacji zapisanych w sesji / cookies.
        public T GetObjectFromJson<T>(HttpContext httpContext, string key)
        {
            // Pobranie informacji z sesji.
            string value = httpContext.Session.GetString(key);

            if (value == null)
            {
                // Pobranie informacji z cookies.
                value = httpContext.Request.Cookies[key];
            }

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }



        // Pobranie informacji zapisanych w JWT.
        public T GetObjectFromJwt<T>(string jwt, string key)
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = jwtHandler.ReadJwtToken(jwt);

            string value = token.Claims.FirstOrDefault(x => x.Type == key).Value;

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            return value == null ? default(T) : (T)converter.ConvertFrom(value); ;
        }



        // Usunięcie sesji / cookies.
        public void Logout(HttpContext httpContext)
        {
            // Usunięcie sesji.
            httpContext.Session.Clear();

            // Usunięcie cookies.
            httpContext.Response.Cookies.Delete("Authorization");
            httpContext.Response.Cookies.Delete("JWToken");
        }
    }
}
