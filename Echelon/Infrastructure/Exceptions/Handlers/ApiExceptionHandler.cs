using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using System.Web.Mvc;
using Echelon.Core.Logging.Loggers;

namespace Echelon.Infrastructure.Exceptions.Handlers
{
    public class ApiExceptionHandler : ExceptionHandler
    {
        /// <summary>When overridden in a derived class, handles the exception asynchronously.</summary>
        /// <returns>A task representing the asynchronous exception handling operation.</returns>
        /// <param name="context">The exception handler context.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public override async Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var clientLogger = DependencyResolver.Current.GetService<IClientLogger>();
            clientLogger.Error(context.Exception.Message);

            var e = context.Exception;
            while (e.InnerException != null) e = e.InnerException;
            await Console.Out.WriteLineAsync(context.Exception.Message);

            var guid = $"Api-Error-{Guid.NewGuid()}";
            clientLogger.Error($"{guid} {e.Message}");
            clientLogger.Debug($"{guid} {e.StackTrace}");

            // Access Exception using context.Exception;  
            string errorMessage = $"Error Handler ID: {guid} Apologies an error has occurred! Some features may not function correctly";
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, errorMessage);

            response.Headers.Add("X-Error", errorMessage);
            context.Result = new ResponseMessageResult(response);
        }
    }
}