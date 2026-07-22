using FluentValidation;

namespace GymAppApi.Application.Gyms.Commands
{
    public class DeleteGymValidator : AbstractValidator<DeleteGymCommand>
    {
        public DeleteGymValidator()
        {
            RuleFor(x => x.GymId)
                .NotEmpty().WithMessage("Gym id required")
                .NotNull().WithMessage("Gym id required");
        }
    }
}
