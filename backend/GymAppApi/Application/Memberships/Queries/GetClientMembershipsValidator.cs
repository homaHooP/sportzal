using FluentValidation;

namespace GymAppApi.Application.Memberships.Queries
{
    public class GetClientMembershipsValidator : AbstractValidator<GetClientMembershipsCommand>
    {
        public GetClientMembershipsValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Client id is required")
                .NotNull().WithMessage("Client id is required");
        }
    }
}
