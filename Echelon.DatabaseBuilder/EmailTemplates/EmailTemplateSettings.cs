using Echelon.Data.Entities.Email;
using Echelon.Misc.Enums;

namespace Echelon.DatabaseBuilder.EmailTemplates
{
    public static class EmailTemplateSettings
    {
        public static EmailTemplateEntity ForgottenPassword => new EmailTemplateEntity
        {
            Subject = "Forgotten Password",
            Body = "Forgot your password?\n No Problem. Just click the link below to reset it\n {{link}}",
            Type = EmailTemplateEnum.ForgottenPassword
        };


        public static EmailTemplateEntity AccountConfirmation => new EmailTemplateEntity
        {
            Subject = "Thanks for Registering",
            Body = "Welcome {{username}},\n Thanks for signing up!\n Please click the link below to confirm your account! {{registerlink}}",
            Type = EmailTemplateEnum.AccountConfirmation
        };
    }
}