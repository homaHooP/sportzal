using FluentValidation;

namespace GymAppApi.Application.Gyms.Queries
{
    public class GetGymByIdValidator: AbstractValidator<GetGymByIdCommand>
    {
        public GetGymByIdValidator()
        {
            RuleFor(x => x.GymId)
                 .NotEmpty().WithMessage("Gym id required")
                 .NotNull().WithMessage("Gym id required");
        }
    }
}
