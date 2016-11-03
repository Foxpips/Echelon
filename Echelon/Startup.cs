using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using Echelon;
using Echelon.Infrastructure.AutoFac;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Echelon
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Injector.RegisterProfilesOnInit();

            var container = Injector.RegisterModulesOnInit();
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.SetDefaultSignInAsAuthenticationType("LoginCookie");
            app.UseExternalSignInCookie();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Login"),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName = "LoginCookie"
            });

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "699010356628-rnulg0m5uer1rg51udpb73v4nqjgn9qn.apps.googleusercontent.com",
                ClientSecret = "q7oDURml260PFcTGAS7VJVLE",
            });

            ConfigureApplication();
        }

        private void ConfigureApplication()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;
        }
    }

    //    public static class Extensions
    //    {
    //        public static IAppBuilder UseAutofacMvc2(this IAppBuilder app)
    //        {
    //            return app.Use(async (context, next) =>
    //            {
    //                var lifetimeScope = context.GetAutofacLifetimeScope();
    //                var httpContext = HttpContext.Current;
    //
    //                if (lifetimeScope != null && httpContext != null)
    //                    httpContext.Items[typeof(ILifetimeScope)] = lifetimeScope;
    //
    //                await next();
    //            });
    //        }
    //    }
}