using System.Web.Mvc;
using System.Web.Routing;

namespace Echelon
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Language Route
            routes.MapRoute(
                name: "DefaultLocalized",
                url: "{lang}/{controller}/{action}/{id}",
                constraints: new {lang = @"(\w{2})|(\w{2}-\w{2})"}, // en or en-US
                defaults: new {controller = "Login", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Token",
                url: "token",
                defaults: new {controller = "Token", action = "Index"}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Login", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}