using FluentValidation;

namespace GymAppApi.Application.Users.Commands
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.")
                .NotNull().WithMessage("Token is required.")
                .MaximumLength(100).WithMessage("Token must not exceed 100 characters.");
        }
    }
}
