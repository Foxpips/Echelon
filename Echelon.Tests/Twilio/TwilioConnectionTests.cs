using System;
using NUnit.Framework;
using Twilio.Auth;
using Twilio.IpMessaging;
using Twilio.IpMessaging.Model;

namespace Echelon.Tests.Twilio
{
    public class TwilioConnectionTests
    {
        [Test]
        [Ignore("Twilio issues")]
        public void Method_Scenario_Result()
        {
            // Find your Account Sid and Auth Token at twilio.com/user/account
            const string accountSid = "AC2006ab427bb0e190f055de1adac711fb";
            const string apikey = "SK4084bf3d1502bcd7d135c8e936fa6870";
            const string apiSecret = "XkS1l3GSlKU3dZJeNya2McvMaRYt3MuH";
            const string credentialSid = "";
            const string channelSid = "";

            // List all members of a channel

            var token = new AccessToken(accountSid, apikey, apiSecret) {Identity = "SimonMarkey"};

            // Create an IP messaging grant for this token
            var grant = new IpMessagingGrant
            {
                EndpointId = "EchelonChat:SimonMarkey:browser",
                ServiceSid = "IS88ae154e76ce4907bd61282b27e295b5"
            };

            token.AddGrant(grant);

            var client = new IpMessagingClient(accountSid, token.ToJWT());
            MemberResult result = client.ListMembers(credentialSid, channelSid);
            Console.WriteLine(result.Members.Count);
        }
    }
}