using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Echelon.Infrastructure.Exceptions.Filters;
using Echelon.Infrastructure.Exceptions.Handlers;

namespace Echelon
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new ApiExceptionFilter());
            config.Services.Replace(typeof(IExceptionHandler), new ApiExceptionHandler());

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