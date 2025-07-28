using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AccountService.Api.SwaggerFilters;

public class RemoveQueryParameters<TTarget>(params string[] propertyNames) : IOperationFilter
{
    public void Apply(OpenApiOperation? operation, OperationFilterContext context)
    {
        if (operation is null)
        {
            return;
        }

        var target = context.MethodInfo.GetParameters().Any(x => x.ParameterType == typeof(TTarget));

        if (!target)
        {
            return;
        }

        operation.Parameters = operation.Parameters
            .Where(x => !propertyNames.Contains(x.Name))
            .ToList();
    }
}
