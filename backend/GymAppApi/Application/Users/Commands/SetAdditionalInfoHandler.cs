using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Domain.Models;
using GymAppApi.Data;
using GymAppApi.Application.Users.Queries;
using FluentValidation.Results;

namespace GymAppApi.Application.Users.Commands
{
    public class SetAdditionalInfoHandler(GymAppDbContext _context, ISender _sender) : IRequestHandler<SetAdditionalInfoCommand,UserDetailsDto>
    {
        public async Task<UserDetailsDto> Handle(SetAdditionalInfoCommand command, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=> x.Id == command.UserId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User", command.UserId);
            }
            if (user.WasDeactivated != null)
                throw new ValidationException(new List<ValidationFailure>
                { new ValidationFailure("User deactivated", "User is deactivated") });

            user.FullName = command.Fullname;
            user.Gender = command.Gender;
            user.Birthday = command.Birthday;

            await _context.SaveChangesAsync(cancellationToken);
            return await _sender.Send(new GetUserByIdCommand { Id = user.Id });
        }
    }
}
