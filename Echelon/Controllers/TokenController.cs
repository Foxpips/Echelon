using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Core.Interfaces.Data;
using Echelon.Objects.Entities.Users;
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
        public async Task<ActionResult> Index(string device)
        {
            // Create a random identity for the client
            var usersEntity = await _dataService.Read<UsersEntity>();
            var externalLoginInfoAsync = _owinContext.Authentication.User;
            var identity = usersEntity.Users.Single(x => x.Email.Equals(externalLoginInfoAsync.Identity.Name)).UserName;

            // Create an Access Token generator
            var token = new AccessToken(AccountSid, ApiKey, ApiSecret) {Identity = identity};

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
                category = "general",
                token = token.ToJWT()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}