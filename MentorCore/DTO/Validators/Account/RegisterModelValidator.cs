using FluentValidation;
using MentorCore.DTO.Account;

namespace MentorCore.DTO.Validators.Account
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("Name can't be null or empty");

            RuleFor(m => m.Email)
                .NotEmpty().WithMessage("Email can't be null or empty")
                .EmailAddress().WithMessage("Incorrect Email address");

            RuleFor(m => m.Password)
                .NotEmpty().WithMessage("Password can't be null or empty");

            RuleFor(m => m.ConfirmPassword)
                .NotEmpty().WithMessage("ConfirmPassword can't be null or empty")
                .Equal(m => m.Password).WithMessage("'ConfirmPassword' and 'Password' do not match.");
        }
    }
}
