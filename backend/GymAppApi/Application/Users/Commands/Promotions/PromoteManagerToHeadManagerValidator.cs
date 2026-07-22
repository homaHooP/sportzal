using FluentValidation;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteManagerToHeadManagerValidator : AbstractValidator<PromoteManagerToHeadManagerCommand>
    {
        public PromoteManagerToHeadManagerValidator()
        {
            RuleFor(x => x.ManagerId)
                .NotEmpty().WithMessage("User id required")
                .NotNull().WithMessage("User id required");
        }
    }
}
