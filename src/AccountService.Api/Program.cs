using AccountService.Abstractions.Constants;
using AccountService.Abstractions.Extensions;
using AccountService.Api;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Api.Scheduler;
using Hangfire;
using System.Reflection;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var configuration = builder.Configuration;
        var environmentName = builder.Environment.EnvironmentName;

        services.AddMockClients();

        services.AddCorsConfiguration();

        services.AddDbContextConfiguration(configuration);

        services.AddExceptionHandler<ExceptionHandler>();

        services.AddAuthConfiguration(configuration);

        services.AddControllerConfiguration();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGenConfiguration(configuration);

        services.AddValidationConfiguration();

        services.AddMediatrConfiguration();

        services.AddHelpers();

        services.AddRepositories();

        services.AddServices();

        services.AddAutoMapper(x => x.AddMaps(Assembly.GetExecutingAssembly()));

        if (environmentName != "Testing")
        {
            services.AddHangfireConfiguration(configuration);
        }

        var app = builder.Build();

        if (!app.Environment.IsEnvironment("Testing"))
        {
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
        }

        app.UseExceptionHandler(_ => { });
        app.MapControllers();
        
        if (!app.Environment.IsEnvironment("Testing"))
        {
            app.UseSwagger();

            var authenticationOptions = configuration.GetRequiredSection(EnvironmentContants.AUTHENTICATION_SECTION).GetRequired<AuthenticationOptions>();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.OAuthClientId(authenticationOptions.ClientId);
                c.OAuthClientSecret(authenticationOptions.ClientSecret);
                c.OAuthUsePkce();
                c.OAuthScopes("openid", "profile");
            });

            app.UseHangfireDashboard("/hangfire");

            JobConfigurator.Configure(app.Services);
        }

        app.Run();
    }
}