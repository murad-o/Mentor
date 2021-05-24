using FluentValidation;
using MentorCore.DTO.Account;

namespace MentorCore.DTO.Validators.Account
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(m => m.Email)
                .NotEmpty().WithMessage("Email can't be null or empty")
                .EmailAddress().WithMessage("Incorrect Email address");

            RuleFor(m => m.Password)
                .NotEmpty().WithMessage("Password can't be null or empty");
        }
    }
}
