using AccountService.Api.Behaviors;
using AccountService.Api.Features.Account;
using AccountService.Api.Features.Statement.GetStatement;
using AccountService.Api.ObjectStorage;
using AccountService.Api.SwaggerFilters;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;

namespace AccountService.Api;

public static class DependencyInjection
{
    private const string REQUEST_ID_PROBLEM_DETAIL = "requestId";
    private const string TRACE_ID_PROBLEM_DETAIL = "traceId";
    private const char HTTP_METHOD_SEPARATOR = ' ';

    public static void AddValidationConfiguration(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static void AddMediatrConfiguration(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    public static void AddControllerConfiguration(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
            });

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    }

    public static void AddSwaggerGenConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(x =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            x.IncludeXmlComments(xmlPath);

            x.OperationFilterDescriptors.Add(new FilterDescriptor
            {
                Type = typeof(RemoveQueryParameters<GetStatementQuery>),
                Arguments = [new[] { nameof(GetStatementQuery.AccountId) }]
            });

            x.OperationFilter<CamelCaseQueryParametersFilter>();
        });
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClientVefiricationService, ClientVefiricationService>();
        services.AddScoped<IAccountStorageService, AccountStorageService>();
    }

    public static void AddHelpers(this IServiceCollection services)
    {
        services.AddSingleton<ICurrencyHelper, CurrencyHelper>();
        services.AddSingleton<IDatetimeHelper, DatetimeHelper>();
    }

    public static void AddProblemsConfiguration(this IServiceCollection services)
    {
        services.AddExceptionHandler<ExceptionHandler>();

        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = string.Join(HTTP_METHOD_SEPARATOR, context.HttpContext.Request.Method, context.HttpContext.Request.Path.Value);
                context.ProblemDetails.Extensions.TryAdd(REQUEST_ID_PROBLEM_DETAIL, context.HttpContext.TraceIdentifier);
                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd(TRACE_ID_PROBLEM_DETAIL, activity?.Id);
            };
        });
    }    
}