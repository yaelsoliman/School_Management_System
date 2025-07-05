using Common.Request.Identity.Token;
using FluentValidation;

namespace SchoolManagementSystem.Validators.Identity.Token
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.Token)
             .NotEmpty().WithMessage("Token is required.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken is required.");

        }
    }
}
