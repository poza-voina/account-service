using Serilog;
using AccountService.Abstractions.Constants;
using AccountService.Abstractions.Extensions;
using AccountService.Api.Behaviors;
using AccountService.Api.Features.Account;
using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.Features.Statements.GetStatement;
using AccountService.Api.Features.Transactions;
using AccountService.Api.Features.Transactions.Interfaces;
using AccountService.Api.ObjectStorage;
using AccountService.Api.ObjectStorage.Events.Consumers;
using AccountService.Api.ObjectStorage.Events.Published;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Api.Scheduler;
using AccountService.Api.Scheduler.Jobs;
using AccountService.Api.SwaggerFilters;
using AccountService.Api.ViewModels.Result;
using AccountService.Infrastructure;
using AccountService.Infrastructure.Repositories;
using AccountService.Infrastructure.Repositories.Interfaces;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;

namespace AccountService.Api;

public static class DependencyInjection
{
    public static void AddEventConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IEventCollector, EventCollector>();
        services.AddScoped<IEventDispatcher, EventDispatcher>();
    }

    public static void AddMockClients(this IServiceCollection services)
    {
        services.AddSingleton<ICollection<Guid>>(_ =>
            [
                Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
                Guid.Parse("a5f5c3b2-1e74-4e6d-9c9d-8bfbec79a222"),
                Guid.Parse("9f8e7d6c-5b4a-3c2d-1e0f-1234567890ab"),
                Guid.Parse("abcdefab-cdef-abcd-efab-cdefabcdef12"),
                Guid.Parse("7c9fcf13-6df1-4eb1-9404-0e4380e2bba5")
            ]);
    }

    public static void AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionSection = configuration.GetRequiredSection(EnvironmentConstants.ConnectionSection);
        var connectionString = connectionSection.GetRequiredValue<string>(EnvironmentConstants.DefaultConnectionStringKey);

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
    }

    public static void AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public static void AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetRequiredSection(EnvironmentConstants.AuthenticationSection).GetRequired<AuthenticationOptions>();

        services
            .AddAuthentication(
                x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(x =>
                {
                    x.Authority = jwtOptions.Authority;
                    x.Audience = jwtOptions.Audience;
                    x.RequireHttpsMetadata = jwtOptions.RequireHttpsMetadata;

                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtOptions.Authority,
                        ValidateIssuer = false,

                        ValidAudience = jwtOptions.Audience,
                        ValidateAudience = false,

                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };

                    x.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            var error = MbResultFactory.WithOperationError("Unauthorized", StatusCodes.Status401Unauthorized);

                            return context.Response.WriteAsJsonAsync(error);
                        }
                    };
                }
            );
    }

    public static void AddValidationConfiguration(this IServiceCollection services)
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static void AddMediatrConfiguration(this IServiceCollection services)
    {
        services.AddMediatR(
            x =>
            {
                x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

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

    public static void AddSwaggerGenConfiguration(this IServiceCollection services, IConfiguration configuration)
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

            var authenticationOptions = configuration.GetRequiredSection(EnvironmentConstants.AuthenticationSection).GetRequired<AuthenticationOptions>();

            x.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(authenticationOptions.AuthorizationUrl),
                        TokenUrl = new Uri(authenticationOptions.TokenUrl),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID scope" },
                            { "profile", "User profile" }
                        }
                    }
                }
            });

            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    ["openid", "profile"]
                }
            });
        });
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClientVerificationService, ClientVerificationService>();
        services.AddScoped<IAccountStorageService, AccountStorageService>();
        services.AddScoped<ITransactionStorageService, TransactionStorageService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IEventFactory, EventFactory>();
        services.AddScoped<IHealthCheckService, HealthCheckService>();
        services.AddScoped<ApplicationContext>();
        services.AddScoped<IEventDispatcher, EventDispatcher>();
    }

    public static void AddAutoMapperConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(x => x.AddMaps(Assembly.GetExecutingAssembly()));
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    public static void AddHelpers(this IServiceCollection services)
    {
        services.AddSingleton<ICurrencyHelper, CurrencyHelper>();
        services.AddSingleton<IDatetimeHelper, DatetimeHelper>();
    }

    public static void AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionSection = configuration.GetRequiredSection(EnvironmentConstants.ConnectionSection);
        var connectionString = connectionSection.GetRequiredValue<string>(EnvironmentConstants.DefaultConnectionStringKey);

        services.AddHangfire(globalConfiguration => globalConfiguration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(x => x.UseNpgsqlConnection(connectionString))
            );

        services.AddHangfireServer();

        services.AddScoped<AccrueInterestJob>();
        services.AddScoped<RabbitMqPublishJob>();
        services.AddSingleton(typeof(JobRunner<>));
    }

    public static void AddRabbitMqConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfiguration = configuration.GetRequiredSection("Brokers:RabbitMq").GetRequired<RabbitMqConfiguration>()
            .Map<AccountOpened>("account.opened")
            .Map<InterestAccrued>("money.*")
            .Map<MoneyCredited>("money.*")
            .Map<MoneyDebited>("money.*")
            .Map<TransferCompleted>("money.*");

        services.AddSingleton(rabbitMqConfiguration);

        services.AddSingleton<RabbitMqConnectionMonitor>();
        services.AddSingleton<IRabbitMqConnectionMonitor>(sp => sp.GetRequiredService<RabbitMqConnectionMonitor>());
        services.AddHostedService(sp => sp.GetRequiredService<RabbitMqConnectionMonitor>());


        services.AddScoped<IRabbitMqConnectionManager, RabbitMqConnectionManager>();

        var consumerConfiguration = new ConsumerConfiguration()
            .WithQueueName("account.antifraud")
            .Map<AntifraudConsumerV1>();

        services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

        services.AddSingleton(consumerConfiguration);
        services.AddHostedService<AntifraudConsumerV1>();
    }

    public static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
    }
}