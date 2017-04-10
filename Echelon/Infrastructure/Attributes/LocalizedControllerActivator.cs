using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace Echelon.Infrastructure.Attributes
{
    public class LocalizedControllerActivator : IControllerActivator
    {
        private readonly string _DefaultLanguage = "en";

        public IController Create(RequestContext requestContext, Type controllerType)
        {
            //Get the {language} parameter in the RouteData
            var lang = requestContext.RouteData.Values["lang"] as string ?? _DefaultLanguage;

            if (lang != _DefaultLanguage)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture =
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                }
                catch
                {
                    throw new NotSupportedException($"ERROR: Invalid language code '{lang}'.");
                }
            }

            return DependencyResolver.Current.GetService(controllerType) as IController;
        }
    }
}