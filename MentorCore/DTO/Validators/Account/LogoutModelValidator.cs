using FluentValidation;
using MentorCore.DTO.Account;

namespace MentorCore.DTO.Validators.Account
{
    public class LogoutModelValidator : AbstractValidator<LogoutModel>
    {
        public LogoutModelValidator()
        {
            RuleFor(m => m.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken can't be null or empty");
        }
    }
}
