using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Gyms.Queries
{
    public class GetGymByIdCommand: IRequest<GymDetailsDto>
    {
        public Guid GymId { get; set; }
    }
}
