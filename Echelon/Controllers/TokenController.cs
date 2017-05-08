using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Data;
using Echelon.Data.Entities.Users;
using Microsoft.Owin;
using Twilio.Auth;

namespace Echelon.Controllers
{
    public class TokenController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IOwinContext _owinContext;

        // Load Twilio configuration from Web.config
        private static string AccountSid => ConfigurationManager.AppSettings["TwilioAccountSid"];

        private static string ApiKey => ConfigurationManager.AppSettings["TwilioApiKey"];

        private static string ApiSecret => ConfigurationManager.AppSettings["TwilioApiSecret"];

        private static string IpmServiceSid => ConfigurationManager.AppSettings["TwilioIpmServiceSid"];

        public TokenController(IDataService dataService, IOwinContext owinContext)
        {
            _owinContext = owinContext;
            _dataService = dataService;
        }

        // GET: /token
        [Authorize]
        public async Task<ActionResult> Index(string device, string channel)
        {
            // Create a random identity for the client
            var user = _owinContext.Authentication.User;
            var userEntity = await _dataService.Single<UserEntity>(x => x.Where(y => y.Email.Equals(user.Identity.Name)));
            var identity = new { username = userEntity.DisplayNameEnabled ? userEntity.DisplayName : userEntity.UserName, email = userEntity.Email };

            // Create an Access Token generator
            var token = new AccessToken(AccountSid, ApiKey, ApiSecret) { Identity = identity.email };

            // Create an IP messaging grant for this token
            var grant = new IpMessagingGrant
            {
                EndpointId = $"EchelonChat:{identity}:{device}",
                ServiceSid = IpmServiceSid
            };

            token.AddGrant(grant);

            return Json(new
            {
                identity,
                category = channel,
                token = token.ToJWT()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}