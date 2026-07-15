using FluentValidation;
using GymAppApi.Application.Gyms.Commands;
using GymAppApi.Application.Users.Commands;

namespace GymAppApi.Application.Users.Validators
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator() { 
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");   
        }
    }
}
