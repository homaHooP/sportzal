using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymAppApi.Application.Common.Exceptions;

namespace GymAppApi.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            var (statusCode, title, detail) = MapException(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions["errors"] = validationException.Errors;
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private static (int StatusCode, string Title, string Detail) MapException(Exception exception)
        {
            return exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest, "Validation error", "One or more validation errors occurred"),
                NotFoundException notFoundEx => (StatusCodes.Status404NotFound, "Not found", notFoundEx.Message),
                ForbiddenAccessException => (StatusCodes.Status403Forbidden, "Forbidden", "You do not have permission to perform this action"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", "Authentication is required"),
                _ => (StatusCodes.Status500InternalServerError, "Server error", "An unexpected error occurred")
            };
        }
    }
}