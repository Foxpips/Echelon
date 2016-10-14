using System.Configuration;
using System.Web.Mvc;
using Faker;
using Twilio.Auth;

namespace Echelon.Controllers
{
    public class TokenController : Controller
    {
        // GET: /token
        public ActionResult Index(string device)
        {
            // Load Twilio configuration from Web.config
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var apiKey = ConfigurationManager.AppSettings["TwilioApiKey"];
            var apiSecret = ConfigurationManager.AppSettings["TwilioApiSecret"];
            var ipmServiceSid = ConfigurationManager.AppSettings["TwilioIpmServiceSid"];

            // Create a random identity for the client
            var identity = Internet.UserName();

            // Create an Access Token generator
            var token = new AccessToken(accountSid, apiKey, apiSecret) {Identity = identity};

            // Create an IP messaging grant for this token
            var grant = new IpMessagingGrant
            {
                EndpointId = $"TwilioChatDemo:{identity}:{device}",
                ServiceSid = ipmServiceSid
            };
            token.AddGrant(grant);

            return Json(new {
                identity,
                token = token.ToJWT()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}