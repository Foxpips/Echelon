using System;
using System.Web.Mvc;
using Echelon.Core.Logging.Loggers;

namespace Echelon.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            var clientLogger = DependencyResolver.Current.GetService<IClientLogger>();

            var newGuid = Guid.NewGuid();
            clientLogger.Error($"Error Guid: {newGuid}");
            clientLogger.Error(filterContext.Exception.Message);
            clientLogger.Error(filterContext.Exception.StackTrace);
            clientLogger.Error(filterContext.Exception.InnerException?.Message);

            filterContext.ExceptionHandled = true;

            // Redirect on error:
            filterContext.Result = RedirectToAction("Index", "Error", new {errorId = newGuid });
        }
    }
}