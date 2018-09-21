using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Echelon.Core.Logging.Loggers;

namespace Echelon.Infrastructure.Exceptions.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var e = actionExecutedContext.Exception;
            while (e.InnerException != null) e = e.InnerException;
            var clientLogger = DependencyResolver.Current.GetService<IClientLogger>();

            var guid = $"Api-Error-{Guid.NewGuid()}";
            clientLogger.Error($"{guid} {e.Message}");
            clientLogger.Debug($"{guid} {e.StackTrace}");
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content =
                    new StringContent(
                        $"Error Filter ID: {guid} Apologies an error has occurred! Some features may not function correctly"),
                ReasonPhrase = "Api Internal Server Error has occurred.",
                StatusCode = HttpStatusCode.InternalServerError,
            };
        }
    }
}