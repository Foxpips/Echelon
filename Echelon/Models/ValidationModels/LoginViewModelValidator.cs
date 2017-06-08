using Echelon.Models.ViewModels;
using FluentValidation;

namespace Echelon.Models.ValidationModels
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Not Valid").NotEmpty().WithMessage("*Required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("*Required").Length(6, 10);
        } 
    }
}