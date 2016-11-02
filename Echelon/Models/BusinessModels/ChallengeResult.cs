using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Echelon.Models.BusinessModels
{
    internal class ChallengeResult : HttpUnauthorizedResult
    {
        private readonly IOwinContext _owinContext;

        public ChallengeResult(string provider, string redirectUri, IOwinContext owinContext)
        {
            _owinContext = owinContext;
            LoginProvider = provider;
            RedirectUri = redirectUri;
        }

        private string LoginProvider { get; }
        private string RedirectUri { get; }

        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            _owinContext.Authentication.Challenge(properties, LoginProvider);
        }
    }
}