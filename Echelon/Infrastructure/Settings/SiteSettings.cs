using System.Configuration;

namespace Echelon.Infrastructure.Settings
{
    public static class SiteSettings
    {
        //Readonly
        public static string AvatarImagesPath => ConfigurationManager.AppSettings["AvatarFilePath"];

        //Constanats
        public const string CookieName = "LoginCookie";
        public const string LoginPath = "/Login";
        public const string GoogleClientId = "502894229257-gvisrn8ie0dfkdec82fjevgcabj24di7.apps.googleusercontent.com";
        public const string GoogleClientSecrect = "6uKMduQZCfzLcyEIIpN106EY";

        public const string GoogleProvider = "Google";
        public const string FacebokProvider = "Facebook";
        public const string GoogleProfileUri = "https://www.googleapis.com/oauth2/v2/userinfo?access_token=";
        public const string FacebookProfileUri = "https://graph.facebook.com/v3.1/{userId}/picture";
        public const string GoogleAccessToken = "urn:google:accesstoken";

        public const string FacebookAppId = "166786510912186";
        public const string FacebookAppSecrect = "53731f53e404bc22e97e6c90c5ea9999";
    }
}