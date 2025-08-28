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

        var errorMessage = exception switch
        {
            NotFoundException => "Ресурс не найден",
            ConflictException => "Ресурс существует",
            DbUpdateConcurrencyException => "Конфлик данных. Запись была изменена другим пользователем",
            _ => null
        };

        MbResult<object> result;
        if (exception is ValidationException validationException)
        {
            result = MbResultFactory.WithValidationErrors(validationException.Errors, statusCode);
        }
        else if (errorMessage is not null)
        {
            result = MbResultFactory.WithOperationError(errorMessage, statusCode);
        }
        else
        {
            result = MbResultFactory.WithOperationError(exception, statusCode);
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }
}