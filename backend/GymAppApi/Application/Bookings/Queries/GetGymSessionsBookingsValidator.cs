using FluentValidation;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetGymSessionsBookingsCommandValidator : AbstractValidator<GetGymSessionsBookingsCommand>
    {
        public GetGymSessionsBookingsCommandValidator()
        {
            RuleFor(x => x.GymId)
                .NotEmpty().WithMessage("Gym id is required")
                .NotNull().WithMessage("Gym id is required");
        }
    }
}