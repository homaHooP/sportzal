using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Users.Commands
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResultDto>
    {
        private readonly GymAppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ISender _sender;

        public RegisterHandler(UserManager<User> userManager, ISender sender, GymAppDbContext context)
        {
            _userManager = userManager;
            _sender = sender;
            _context = context;
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

            await using var transaction = await _context.Database.BeginTransactionAsync(ct);

            var result = await _userManager.CreateAsync(newUser, request.password);
            if (!result.Succeeded)
                throw new ValidationException(
                    result.Errors.Select(e => new ValidationFailure("Password", e.Description)).ToList()
                );

            var roleResult = await _userManager.AddToRoleAsync(newUser, RoleNames.Client);
            if (!roleResult.Succeeded)
                throw new ValidationException(
                    roleResult.Errors.Select(e => new ValidationFailure("Role", e.Description)).ToList()
                );

            var client = new UserClient { UserId = newUser.Id };
            _context.UserClients.Add(client);
            await _context.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

            return await _sender.Send(new LoginCommand
            {
                Email = request.email,
                Password = request.password
            }, ct);
        }
    }
}