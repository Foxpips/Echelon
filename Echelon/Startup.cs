using Echelon;
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
            app.SetDefaultSignInAsAuthenticationType("GoogleCookie");

            app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    LoginPath = new PathString("/Account/Login"),
                    AuthenticationType = "GoogleCookie"
                });

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                {
                    ClientId = "699010356628-rnulg0m5uer1rg51udpb73v4nqjgn9qn.apps.googleusercontent.com",
                    ClientSecret = "q7oDURml260PFcTGAS7VJVLE"
            });
        }
    }
}