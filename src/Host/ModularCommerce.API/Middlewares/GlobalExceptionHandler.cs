using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ModularCommerce.API.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, problemDetail) = exception switch
            {
                FluentValidation.ValidationException validationException => (
                    StatusCodes.Status400BadRequest,
                    new ValidationProblemDetails(
                        validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                    )
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "One or more validation errors occured."
                    }),

                _ => (
                StatusCodes.Status500InternalServerError,
                new ProblemDetails()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal server error. Try again later."
                })
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                logger.LogError(exception, "Unexpected internal error occured");
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync((object)problemDetail, cancellationToken);

            return true;
        }
    }
}
