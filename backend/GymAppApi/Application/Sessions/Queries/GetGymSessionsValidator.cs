using FluentValidation;

namespace GymAppApi.Application.Sessions.Queries
{
    public class GetGymSessionsValidator : AbstractValidator<GetGymSessionsCommand>
    {
        public GetGymSessionsValidator()
        {
            RuleFor(x=> x.GymId)
                .NotEmpty().WithMessage("Gym id is required")
                .NotNull().WithMessage("Gym id is required");
        }
    }
}
