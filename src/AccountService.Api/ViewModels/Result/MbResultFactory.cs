using FluentValidation.Results;

namespace AccountService.Api.ViewModels.Result;

public static class MbResultFactory
{
    public static MbResult<object> WithValidationErrors(IEnumerable<ValidationFailure> errors, int statusCode)
    {
        return new MbResult<object>
        {
            ValidationErrors = errors.Select(
            x => new Error
            {
                Field = x.PropertyName,
                Message = x.ErrorMessage
            }),
            StatusCode = statusCode
        };
    }
    public static MbResult<object> WithOperationError(Exception exception, int statusCode)
    {
        return new MbResult<object>
        {
            OperationError = new OperationError
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                ExceptionType = exception.GetType().FullName
            },
            StatusCode = statusCode
        };
    }

    public static MbResult<T> WithSuccess<T>(T? data)
    {
        return new MbResult<T>
        {
            Result = data,
            StatusCode = StatusCodes.Status200OK
        };
    }
}
