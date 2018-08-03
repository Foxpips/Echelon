using Echelon.Data.Entities.Email;
using Echelon.Misc.Enums;

namespace Echelon.DatabaseBuilder.EmailTemplates
{
    public static class EmailTemplateSettings
    {
        public static EmailTemplateEntity ResetPassword => new EmailTemplateEntity
        {
            Subject = "Forgotten Password",
            Body = "Forgot your password?\n No Problem. Just click the link below to reset it\n {{link}} \n If you did not request to have your password reset please ignore this email.",
            Type = EmailTemplateEnum.ResetPassword
        };

        public static EmailTemplateEntity AccountConfirmation => new EmailTemplateEntity
        {
            Subject = "Thanks for Registering",
            Body = "Welcome {{username}},\n Thanks for signing up!\n Please click the link below to confirm your account!\n {{link}}",
            Type = EmailTemplateEnum.AccountConfirmation
        };
    }
}