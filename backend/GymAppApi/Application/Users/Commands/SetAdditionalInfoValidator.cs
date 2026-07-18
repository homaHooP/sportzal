using FluentValidation;

namespace GymAppApi.Application.Users.Commands
{
    public class SetAdditionalInfoValidator : AbstractValidator<SetAdditionalInfoCommand>
    {
        public SetAdditionalInfoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id required");

            RuleFor(x => x.Fullname)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .MaximumLength(50).WithMessage("Get a job plz");

            RuleFor(x => x.Birthday)
                .NotNull().WithMessage("Birthday is required")
                .NotEqual(DateTime.MinValue).WithMessage("Birthday is required")
                .LessThanOrEqualTo(DateTime.UtcNow.AddYears(-4)).WithMessage("Please enter a valid birthday")
                .GreaterThan(DateTime.UtcNow.AddYears(-120)).WithMessage("Please enter a valid birthday");
        }
    }
}