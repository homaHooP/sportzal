using FluentValidation;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetSessionBookingsCommandValidator : AbstractValidator<GetSessionBookingsCommand>
    {
        public GetSessionBookingsCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("Session id is required")
                .NotNull().WithMessage("Session id is required");
        }
    }
}