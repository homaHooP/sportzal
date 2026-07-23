using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Domain.Models;
using GymAppApi.Data;
using GymAppApi.Application.Users.Queries;
using FluentValidation.Results;
using GymAppApi.Services.Token;

namespace GymAppApi.Application.Users.Commands
{
    public class SetAdditionalInfoHandler(UserManager<User> _userManager, ITokenService _tokenService) : IRequestHandler<SetAdditionalInfoCommand,AuthResultDto>
    {
        public async Task<AuthResultDto> Handle(SetAdditionalInfoCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x=> x.Id == command.UserId, cancellationToken);
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

            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenService.GenerateAccessToken(user, roles);
            return new AuthResultDto(accessToken, null);
        }
    }
}
