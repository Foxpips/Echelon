using Echelon.Mediators;
using FluentValidation;

namespace Echelon.Models.ValidationModels
{
    public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(x => x.Password).Length(6, 20).NotEmpty().WithMessage("*Required");
        }
    }
}