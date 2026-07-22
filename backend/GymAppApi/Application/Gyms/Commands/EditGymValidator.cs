using FluentValidation;

namespace GymAppApi.Application.Gyms.Commands
{
    public class EditGymValidator : AbstractValidator<EditGymCommand>
    {
        public EditGymValidator()
        {
            RuleFor(x => x.GymId)
                 .NotEmpty().WithMessage("Gym id required")
                 .NotNull().WithMessage("Gym id required");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Gym name is required.")
                .MaximumLength(50).WithMessage("Gym name cannot exceed 50 characters.")
                .MinimumLength(4).WithMessage("Gym name must be at least 4 characters long.");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Gym address is required.")
                .Matches("^[а-яА-ЯіІїЇєЄa-zA-Z0-9\\s\\.,\\-\\/№]{5,200}$").WithMessage("Gym address must be between 5 and 200 characters and can only contain letters, numbers and spaces.");
        }
    }
}
