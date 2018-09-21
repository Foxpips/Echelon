using Echelon.Models.ViewModels;
using FluentValidation;

namespace Echelon.Models.ValidationModels
{
    public class ProfileViewModelValidator : AbstractValidator<ProfileViewModel>
    {
        public ProfileViewModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("*Required");
        }
    }
}