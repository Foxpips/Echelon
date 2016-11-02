using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Core.Interfaces.Data;
using Echelon.Entities.Users;
using Faker;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Twilio.Auth;

namespace Echelon.Controllers
{
    public class TokenController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IOwinContext _owinContext;

        public TokenController(IDataService dataService, IOwinContext owinContext)
        {
            _owinContext = owinContext;
            _dataService = dataService;
        }

        // GET: /token
        public async Task<ActionResult> Index(string device)
        {
            // Load Twilio configuration from Web.config
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var apiKey = ConfigurationManager.AppSettings["TwilioApiKey"];
            var apiSecret = ConfigurationManager.AppSettings["TwilioApiSecret"];
            var ipmServiceSid = ConfigurationManager.AppSettings["TwilioIpmServiceSid"];

            // Create a random identity for the client
            var usersEntity = await _dataService.Read<UsersEntity>();
            var externalLoginInfoAsync = _owinContext.Authentication.User;
//            var identity = usersEntity.Users.Single(x => x.Email.Equals(externalLoginInfoAsync.Email)).UserName;
            var identity = externalLoginInfoAsync.Identity.Name;

            // Create an Access Token generator
            var token = new AccessToken(accountSid, apiKey, apiSecret) { Identity = identity };

            // Create an IP messaging grant for this token
            var grant = new IpMessagingGrant
            {
                EndpointId = $"EchelonChat:{identity}:{device}",
                ServiceSid = ipmServiceSid
            };
            token.AddGrant(grant);

            return Json(new
            {
                identity,
                token = token.ToJWT()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}