using FluentValidation;

namespace GymAppApi.Application.Bookings.Commands
{
    public class CancelBookingValidator : AbstractValidator<CancelBookingCommand>
    {
        public CancelBookingValidator()
        {
            RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage("Booking id is required")
                .NotNull().WithMessage("Booking id is required");
        }
    }
}