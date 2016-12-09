﻿using System.Reflection;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Echelon;
using Echelon.Core.Extensions.Autofac;
using Echelon.Core.Extensions.AutoMapper;
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

            var container = builder.RegisterCustomModules().Build();
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            AutoMapperExtensions.RegisterProfilesOnInit(targetAssembly);

            ConfigureCookies(app);
            ConfigureApplication();
            StartServiceBus(container);
        }

        private static void StartServiceBus(IContainer container)
        {
            var bus = container.Resolve<IBusControl>();
            bus.StartAsync();
        }

        private static void ConfigureCookies(IAppBuilder app)
        {
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
        }

        private static void ConfigureApplication()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;
        }
    }
}