using FluentValidation;

namespace GymAppApi.Application.Bookings.Commands
{
    public class BookASessionCommandValidator : AbstractValidator<BookASessionCommand>
    {
        public BookASessionCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("Session id is required")
                .NotNull().WithMessage("Session id is required");
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Client id is required")
                .NotNull().WithMessage("Client id is required"); ;
        }
    }
}