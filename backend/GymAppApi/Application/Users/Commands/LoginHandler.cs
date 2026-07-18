using GymAppApi.Data;
using GymAppApi.Domain.Models;
using GymAppApi.Services.Token;
using MediatR;
using Microsoft.AspNetCore.Identity;
using GymAppApi.Domain.DTO;
using GymAppApi.Application.Common.Exceptions;
using FluentValidation.Results;

namespace GymAppApi.Application.Users.Commands
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResultDto>
    {
        private readonly GymAppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public LoginHandler(GymAppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Form", "Invalid email or password")
                });

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Form", "Invalid email or password")
                });

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(ct);

            return new AuthResultDto(accessToken,refreshToken.Token);
        }
    }
}
