using FluentValidation;

namespace GymAppApi.Application.Users.Commands
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidator() { 
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id required")
                .NotNull().WithMessage("User id required");
        }
    }
}
