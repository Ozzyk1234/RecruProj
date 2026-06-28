using Microsoft.AspNetCore.Diagnostics;

namespace RecruProj.Validators.GlobalExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger;
        }
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var exceptionMessage = exception.Message;
            logger.LogError(
                "Error Message: {exceptionMessage}, Time of occurrence {time}, Status Code: {statusCode}",
                exceptionMessage, DateTime.UtcNow, httpContext.Response.StatusCode);
            return ValueTask.FromResult(true);
        }
    }
}
