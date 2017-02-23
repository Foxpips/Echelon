namespace Echelon.Infrastructure.Settings
{
    public static class SiteSettings
    {
        public static string CookieName => "LoginCookie";
        public static string LoginPath => "/Login";
        public static string GoogleClientId => "699010356628-rnulg0m5uer1rg51udpb73v4nqjgn9qn.apps.googleusercontent.com";
        public static string GoogleClientSecrect => "q7oDURml260PFcTGAS7VJVLE";

        public static string GoogleProvider = "Google";
        public static string GoogleProfileUri = "https://www.googleapis.com/oauth2/v2/userinfo?access_token=";
        public static string GoogleAccessToken = "urn:google:accesstoken";
    }
}