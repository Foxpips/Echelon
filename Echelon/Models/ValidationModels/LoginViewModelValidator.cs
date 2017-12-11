using Echelon.Models.ViewModels;
using FluentValidation;

namespace Echelon.Models.ValidationModels
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().WithMessage("*Required");
            RuleFor(x => x.Password).Length(6, 20).NotEmpty().WithMessage("*Required");
        }
    }
}