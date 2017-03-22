using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Echelon;
using Echelon.Core.Extensions.Autofac;
using Echelon.Infrastructure.Attributes;
using Echelon.Infrastructure.Settings;
using MassTransit;
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
            var targetAssembly = GetType().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterControllers(targetAssembly);
            builder.RegisterApiControllers(targetAssembly);

            var container = builder.RegisterCustomModules(true, targetAssembly).Build();
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            ControllerBuilder.Current.SetControllerFactory(
                new DefaultControllerFactory(new LocalizedControllerActivator()));

            ConfigureCookies(app);
            ConfigureApplication();
        }

        private static void ConfigureApplication()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;
        }

        private static void ConfigureCookies(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(SiteSettings.CookieName);
            app.UseExternalSignInCookie();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString(SiteSettings.LoginPath),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName = SiteSettings.CookieName
            });

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = SiteSettings.GoogleClientId,
                ClientSecret = SiteSettings.GoogleClientSecrect,
                Scope = { "profile email" },
                Provider = new GoogleOAuth2AuthenticationProvider
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new Claim("urn:google:name",
                            context.Identity.FindFirstValue(ClaimTypes.Name)));
                        context.Identity.AddClaim(new Claim("urn:google:email",
                            context.Identity.FindFirstValue(ClaimTypes.Email)));
                        context.Identity.AddClaim(new Claim("urn:google:accesstoken", context.AccessToken,
                            ClaimValueTypes.String, "Google"));

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}