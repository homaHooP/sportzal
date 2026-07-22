using FluentValidation;
using GymAppApi.Services;
using GymAppApi.Domain.Constants;

namespace GymAppApi.Application.Users.Commands.Outdated
{
    public class DeleteUserLastRoleValidator : AbstractValidator<DeleteUserRoleCommand>
    {
        public DeleteUserLastRoleValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id is required")
                .NotNull().WithMessage("User id is required");
            RuleFor(x=>x.Role)
                .NotEmpty().WithMessage("User role is required")
                .NotNull().WithMessage("User role is required")
                .Must(role => RoleNames.All.Contains(role)).WithMessage("Invalid role");
        }
    }
}
