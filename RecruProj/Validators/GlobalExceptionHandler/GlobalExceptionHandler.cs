using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RecruProj.Validators.GlobalExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            ProblemDetails problem;

            switch(exception)
            {
                case ConflictException conflictException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Konflikt zasobu. Duplikat",
                        Detail = conflictException.Message
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                    await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
                    return true;
                case NotFoundException notFoundException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Zasób nie znaleziony.",
                        Detail = notFoundException.Message
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
                    return true;
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Wystąpił błąd serwera.",
                        Detail = exception.Message
                    }, cancellationToken);
                    return true;
            }
        }
    }
}