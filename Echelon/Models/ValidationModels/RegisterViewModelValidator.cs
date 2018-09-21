using Echelon.Models.ViewModels;
using FluentValidation;

namespace Echelon.Models.ValidationModels
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().WithMessage("*Required");
            RuleFor(x => x.Password).Length(6, 20).NotEmpty().WithMessage("*Required");
            RuleFor(x => x.UserName).Length(1, 20).NotEmpty().WithMessage("*Required");
        }
    }
}