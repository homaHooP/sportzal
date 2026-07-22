using FluentValidation;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteClientToTrainerValidator : AbstractValidator<PromoteClientToTrainerCommand>
    {
        public PromoteClientToTrainerValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Client id is required")
                .NotNull().WithMessage("Client id is required");
            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("Trainer specialty required")
                .NotNull().WithMessage("Trainer specialty required")
                .MaximumLength(30).WithMessage("Trainer specialty is too long")
                .MinimumLength(4).WithMessage("Trainer specialty is too short");
        }
    }
}
