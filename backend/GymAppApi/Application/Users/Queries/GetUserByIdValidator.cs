using FluentValidation;

namespace GymAppApi.Application.Users.Queries
{
    public class GetUserByIdValidator : AbstractValidator<GetUserByIdCommand>
    {
        public GetUserByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id required")
                .NotNull().WithMessage("Id required");
        }
    }
}
