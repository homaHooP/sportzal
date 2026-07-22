using FluentValidation;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteTrainerToManagerValidator: AbstractValidator<PromoteTrainerToManagerCommand>
    {
        public PromoteTrainerToManagerValidator()
        {
            RuleFor(x => x.TrainerId)
                .NotEmpty().WithMessage("User id required")
                .NotNull().WithMessage("User id required");
            RuleFor(x => x.GymId)
                .NotEmpty().WithMessage("User id required")
                .NotNull().WithMessage("User id required");
        }
    }
}
