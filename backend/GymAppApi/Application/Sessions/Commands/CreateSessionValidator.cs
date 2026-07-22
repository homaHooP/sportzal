using FluentValidation;
using GymAppApi.Domain.Enums;

namespace GymAppApi.Application.Sessions.Commands
{
    public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
    {
        public CreateSessionCommandValidator()
        {
            RuleFor(x => x.GymId).NotEmpty();
            RuleFor(x => x.TrainerId).NotEmpty();
            RuleFor(x => x.Type).IsInEnum();

            RuleFor(x => x.StartTime)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Start time must be in the future");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.MaxParticipants)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.MaxParticipants)
                .Equal(1)
                .When(x => x.Type == SessionType.Individual)
                .WithMessage("Individual session must have exactly 1 max participant");

            RuleFor(x => x.MaxParticipants)
                .GreaterThan(1)
                .When(x => x.Type == SessionType.Group)
                .WithMessage("Group session must have more than 1 max participant");
        }
    }
}