using FluentValidation;
using MediatR;

namespace AccountService.Api.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var failures = validators.Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x is not null).ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return next(cancellationToken);
    }
}
