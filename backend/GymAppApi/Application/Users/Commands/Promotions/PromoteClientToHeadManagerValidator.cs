using FluentValidation;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteClientToHeadManagerValidator : AbstractValidator<PromoteClientToHeadManagerCommand>
    {
        public PromoteClientToHeadManagerValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("User id required")
                .NotNull().WithMessage("User id required");
        }
    }
}
