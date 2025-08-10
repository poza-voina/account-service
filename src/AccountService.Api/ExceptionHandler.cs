using AccountService.Abstractions.Exceptions;
using AccountService.Api.ViewModels.Result;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Api;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            ConflictException => StatusCodes.Status409Conflict,
            DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
            UnprocessableException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

        MbResult<object> result;
        if (exception is ValidationException validationException)
        {
            result = MbResultFactory.WithValidationErrors(validationException.Errors, statusCode);
        }
        else
        {
            result = MbResultFactory.WithOperationError(exception, statusCode);
        }

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }
}