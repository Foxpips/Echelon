using System;
using System.Web;

namespace Echelon.Infrastructure.Helpers
{
    public class CookieStore
    {
        public static void SetCookie(string key, string value, TimeSpan expires)
        {
            var cookie = new HttpCookie(key, value) {Expires = DateTime.Now.Add(expires)};
            HttpContext.Current.Response.Cookies.Remove(cookie.Name);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetCookie(string key)
        {
            return HttpContext.Current.Request.Cookies.Get(key)?.Value;
        }
    }
}