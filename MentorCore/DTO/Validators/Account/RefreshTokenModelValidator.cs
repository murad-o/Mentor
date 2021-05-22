using FluentValidation;
using MentorCore.DTO.Account;

namespace MentorCore.DTO.Validators.Account
{
    public class RefreshTokenModelValidator : AbstractValidator<RefreshTokenModel>
    {
        public RefreshTokenModelValidator()
        {
            RuleFor(m => m.AccessToken)
                .NotEmpty().WithMessage("AccessToken can't be null or empty");

            RuleFor(m => m.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken can't be null or empty");
        }
    }
}
