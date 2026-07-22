using FluentValidation;

namespace GymAppApi.Application.Memberships.Commands
{
    public class AssignMembershipCommandValidator : AbstractValidator<AssignMembershipCommand>
    {
        public AssignMembershipCommandValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty()
                .WithMessage("ClientId is required.");

            RuleFor(x => x.GymId)
                .NotEmpty()
                .WithMessage("GymId is required.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid membership type.");

            RuleFor(x => x.EndDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("End date must be in the future.");

            RuleFor(x => x.SessionsLeft)
                .GreaterThan(0)
                .WithMessage("Sessions left cannot be negative.");
        }
    }
}