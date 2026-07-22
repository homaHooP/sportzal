using FluentValidation;

namespace GymAppApi.Application.Gyms.Commands
{
    public class DeleteGymManagerValidator : AbstractValidator<DeleteGymManagerCommand>
    {
        public DeleteGymManagerValidator()
        {
            RuleFor(x => x.GymId)
                 .NotEmpty().WithMessage("Gym id required")
                 .NotNull().WithMessage("Gym id required");
            RuleFor(x => x.ManagerId)
                 .NotEmpty().WithMessage("Gym id required")
                 .NotNull().WithMessage("Gym id required");
        }
    }
}
