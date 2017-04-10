namespace Echelon.Infrastructure.Settings
{
    public static class SiteSettings
    {
        public static string CookieName => "LoginCookie";
        public static string LoginPath => "/Login";

        public static string GoogleClientId
            => "502894229257-gvisrn8ie0dfkdec82fjevgcabj24di7.apps.googleusercontent.com";

        public static string GoogleClientSecrect => "6uKMduQZCfzLcyEIIpN106EY";

        public static string GoogleProvider = "Google";
        public static string GoogleProfileUri = "https://www.googleapis.com/oauth2/v2/userinfo?access_token=";
        public static string GoogleAccessToken = "urn:google:accesstoken";
    }
}