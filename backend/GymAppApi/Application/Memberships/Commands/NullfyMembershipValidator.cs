using FluentValidation;

namespace GymAppApi.Application.Memberships.Commands
{
    public class NullfyMembershipValidator : AbstractValidator<NullifyMembershipCommand>
    {
        public NullfyMembershipValidator()
        {
            RuleFor(x => x.MembershipId)
                .NotEmpty().WithMessage("Membership id is required")
                .NotNull().WithMessage("Membership id is required");
        }
    }
}
