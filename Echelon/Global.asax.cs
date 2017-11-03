using System;
using System.Globalization;
using System.IO;
using System.Web;
using Echelon.Core.Logging.Loggers;

namespace Echelon
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var mapPath = Server.MapPath("~/log");
            var errorFile = Path.Combine(currentDirectory, DateTime.Today.ToString(CultureInfo.InvariantCulture));

            var clientLogger = new ClientLogger(errorFile);
            var lastError = Server.GetLastError();
            clientLogger.Error(lastError.Message);
            clientLogger.Error(lastError.InnerException?.Message);
            clientLogger.Error(lastError.InnerException?.StackTrace);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}