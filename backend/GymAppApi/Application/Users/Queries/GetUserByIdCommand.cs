using GymAppApi.Domain.DTO;
using MediatR;
using GymAppApi.Application.Common.Exceptions;
using FluentValidation.Results;
using GymAppApi.Application.Common;

namespace GymAppApi.Application.Users.Queries
{
    public class GetUserByIdCommand : IRequest<UserDetailsDto>, IClientOwnedRequest
    {
        public Guid Id { get; set; }
        public Guid GetOwnerId() => Id;
    }
}
