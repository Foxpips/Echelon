using Echelon.Models.ViewModels;
using FluentValidation;

namespace Echelon.Models.ValidationModels
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgottenPasswordModel>
    {
        public ForgotPasswordViewModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().WithMessage("*Required");
        }
    }
}