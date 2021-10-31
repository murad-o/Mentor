using Contracts.Account;
using FluentValidation;

namespace Contracts.Validators.Account
{
    public class EmailConfirmationModelValidator : AbstractValidator<EmailConfirmationModel>
    {
        public EmailConfirmationModelValidator()
        {
            RuleFor(m => m.Email)
                .NotEmpty().WithMessage("Email can't be null or empty")
                .EmailAddress().WithMessage("Incorrect Email address");

            RuleFor(m => m.Token)
                .NotEmpty().WithMessage("Token can't be null or empty");
        }
    }
}
