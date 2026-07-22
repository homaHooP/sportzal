using FluentValidation;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteClientToManagerValidator : AbstractValidator<PromoteClientToManagerCommand>
    {
        public PromoteClientToManagerValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("User id required")
                .NotNull().WithMessage("User id required");
            RuleFor(x => x.GymId)
                .NotEmpty().WithMessage("User id required")
                .NotNull().WithMessage("User id required");
        }
    }
}
