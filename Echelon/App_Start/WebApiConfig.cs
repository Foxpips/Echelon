using System.Web.Http;

namespace Echelon
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // Map this rule first
            config.Routes.MapHttpRoute(
                "WithActionApi",
                "api/{controller}/{action}/{person}",
                defaults: new {person = RouteParameter.Optional}
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
        }
    }
}