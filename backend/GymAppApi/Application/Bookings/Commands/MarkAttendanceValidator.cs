using FluentValidation;
using GymAppApi.Domain.Enums;

namespace GymAppApi.Application.Bookings.Commands
{
    public class MarkAttendanceValidator : AbstractValidator<MarkAttendanceCommand>
    {
        public MarkAttendanceValidator()
        {
            RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage("Booking id is required")
                .NotNull().WithMessage("Booking id is required");

            RuleFor(x => x.Status)
                .Must(s => s == BookingStatus.Attended || s == BookingStatus.NoShow)
                .WithMessage("Status must be either Attended or NoShow");
        }
    }
}