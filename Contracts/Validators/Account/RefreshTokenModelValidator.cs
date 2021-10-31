using Contracts.Account;
using FluentValidation;

namespace Contracts.Validators.Account
{
    public class RefreshTokenModelValidator : AbstractValidator<JwtTokenModel>
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
