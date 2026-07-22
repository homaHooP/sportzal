using FluentValidation;

namespace GymAppApi.Application.Sessions.Commands
{
    public class CancelSessionValidator : AbstractValidator<CancelSessionCommand>
    {
        public CancelSessionValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("Session id is required")
                .NotNull().WithMessage("Session id is required");
        }
    }
}
