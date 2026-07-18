using GymAppApi.Application.Common;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Services;
using MediatR;
using GymAppApi.Domain.Constants;

public class AuthorizationBehavior<TRequest, TResponse>(CurrentUserService _curUser)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IClientOwnedRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_curUser.UserId == Guid.Empty || request.GetOwnerId() != _curUser.UserId && !_curUser.IsInRole(RoleNames.Manager) && !_curUser.IsInRole(RoleNames.HeadManager))
        {
            throw new ForbiddenAccessException("");
        }
        return await next();
    }
}