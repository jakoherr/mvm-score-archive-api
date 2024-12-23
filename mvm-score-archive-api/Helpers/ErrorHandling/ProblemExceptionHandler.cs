using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.Exceptions;

namespace Mvm.Score.Archive.Api.Helpers.ErrorHandling;

public class ProblemExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService problemDetailsService;

    public ProblemExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        this.problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = new();

        switch (exception)
        {
            case NotFoundException notFoundException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = notFoundException.Error,
                    Detail = notFoundException.Message,
                    Type = "Not Found",
                };
                break;

            default:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exception.Message,
                    Type = "Internal Server Error",
                };
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        return await this.problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
            });
    }
}
