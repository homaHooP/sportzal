using FluentValidation;
using GymAppApi.Domain.Constants;

namespace GymAppApi.Application.Users.Commands
{
    public class GiveUserRoleValidator : AbstractValidator<GiveUserRoleCommand>
    {
        private static readonly string[] ValidRoles = RoleNames.All;
        public GiveUserRoleValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Id is required")
                .NotNull().WithMessage("Id is required");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .NotNull().WithMessage("Role is required")
                .Must(role => ValidRoles.Contains(role)).WithMessage("Invalid role");
        }
    }
}
