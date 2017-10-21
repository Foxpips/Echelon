﻿using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Data;
using Echelon.Data.Entities.Users;
using Microsoft.Owin;
using Twilio.Auth;

namespace Echelon.Mediators
{
    public class TokenMediator : IMediator
    {

        private readonly IDataService _dataService;
        private readonly IOwinContext _owinContext;

        // Load Twilio configuration from Web.config
        private static string AccountSid => ConfigurationManager.AppSettings["TwilioAccountSid"];

        private static string ApiKey => ConfigurationManager.AppSettings["TwilioApiKey"];

        private static string ApiSecret => ConfigurationManager.AppSettings["TwilioApiSecret"];

        private static string IpmServiceSid => ConfigurationManager.AppSettings["TwilioIpmServiceSid"];

        public TokenMediator(IDataService dataService, IOwinContext owinContext)
        {
            _owinContext = owinContext;
            _dataService = dataService;
        }

        public async Task<object> CreateToken(string device, string channel)
        {
            // Create a random identity for the client
            var user = _owinContext.Authentication.User;
            var userEntity = await _dataService.Single<UserEntity>(x => x.Where(y => y.Email.Equals(user.Identity.Name)));
            var identity = new { username = userEntity.DisplayName, uniqueuserid = userEntity.UniqueIdentifier };

            // Create an Access Token generator
            var token = new AccessToken(AccountSid, ApiKey, ApiSecret) { Identity = identity.uniqueuserid };

            // Create an IP messaging grant for this token
            token.AddGrant(new IpMessagingGrant
            {
                EndpointId = $"EchelonChat:{identity}:{device}",
                ServiceSid = IpmServiceSid
            });

            return new
            {
                identity,
                category = channel,
                token = token.ToJWT()
            };
        }
    }
}