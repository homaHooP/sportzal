using FluentValidation;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetClientBookingsValidator : AbstractValidator<GetClientBookingsCommand>
    {
        public GetClientBookingsValidator()
        {
            RuleFor(x=>x.ClientId)
                .NotEmpty().WithMessage("Client id is required")
                .NotNull().WithMessage("Client id is required");
        }
    }
}
