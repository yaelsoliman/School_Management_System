using Common.Request.Identity.Token;
using FluentValidation;

namespace SchoolManagementSystem.Validators.Identity.Token
{
    public class TokenValidator : AbstractValidator<TokenRequest>
    {
        public TokenValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
