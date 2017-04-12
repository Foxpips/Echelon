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
        public const string GoogleProfileUri = "https://www.googleapis.com/oauth2/v2/userinfo?access_token=";
        public const string GoogleAccessToken = "urn:google:accesstoken";
    }
}