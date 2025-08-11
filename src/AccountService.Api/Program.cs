using AccountService.Abstractions.Constants;
using AccountService.Abstractions.Extensions;
using AccountService.Api;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Api.Scheduler;
using Hangfire;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

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

services.AddHangfireConfiguration(configuration);

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(_ => { });
app.MapControllers();
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

app.Run();