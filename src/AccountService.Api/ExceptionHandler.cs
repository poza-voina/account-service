using AccountService.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AccountService.Api;

public class ExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Title = nameof(Results.InternalServerError),
            Detail = exception.Message,
            Status = StatusCodes.Status500InternalServerError
        };

        switch (exception)
        {
            case NotFoundException:
                problemDetails.Title = nameof(Results.NotFound);
                problemDetails.Status = StatusCodes.Status404NotFound;
                break;
            case ValidationException:
                problemDetails.Title = nameof(Results.BadRequest);
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;
            case ConflictException:
                problemDetails.Title = nameof(Results.Conflict);
                problemDetails.Status = StatusCodes.Status409Conflict;
                break;
            case UnprocessableException:
                problemDetails.Title = nameof(Results.UnprocessableEntity);
                problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                break;
        }

        if (problemDetails is not { Status: not null })
        {
            return true;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });

    }
}