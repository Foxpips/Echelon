using Echelon.Data.Entities.Email;
using Echelon.Misc.Enums;

namespace Echelon.DatabaseBuilder.EmailTemplates
{
    public static class EmailTemplateSettings
    {
        public static EmailTemplateEntity ResetPassword => new EmailTemplateEntity
        {
            Subject = "Reset Password",
            Body = "In order to reset your password simply click the link below\n {{link}} \nIf you did not request to have your password reset please ignore this email.",
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