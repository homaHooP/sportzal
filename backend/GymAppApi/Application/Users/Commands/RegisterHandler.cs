using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GymAppApi.Application.Users.Commands
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResultDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly ISender _sender;

        public RegisterHandler(UserManager<User> userManager, ISender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        public async Task<AuthResultDto> Handle(RegisterCommand request, CancellationToken ct)
        {
            var exists = await _userManager.FindByEmailAsync(request.email);
            if (exists != null)
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Email", "User with this email already exists")
                });

            var newUser = new User
            {
                UserName = request.email.Split("@")[0],
                Email = request.email
            };

            var result = await _userManager.CreateAsync(newUser, request.password);
            if (!result.Succeeded)
                throw new ValidationException(
                    result.Errors.Select(e => new ValidationFailure("Password", e.Description)).ToList()
                );

            var roleResult = await _userManager.AddToRoleAsync(newUser, "Client");
            if (!roleResult.Succeeded)
                throw new ValidationException(
                    roleResult.Errors.Select(e => new ValidationFailure("Role", e.Description)).ToList()
                );

            return await _sender.Send(new LoginCommand
            {
                Email = request.email,
                Password = request.password
            }, ct);
        }
    }
}