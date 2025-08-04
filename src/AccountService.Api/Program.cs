using AccountService.Api;
using AccountService.Api.Extensions;
using AccountService.Api.ObjectStorage;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


services.AddCorsConfiguration();

services.AddExceptionHandler<ExceptionHandler>();

services.AddAuthConfiguration(configuration);

services.AddControllerConfiguration();

services.AddEndpointsApiExplorer();

services.AddSwaggerGenConfiguration(configuration);

services.AddValidationConfiguration();

services.AddMediatrConfiguration();

services.AddHelpers();

services.AddServices();

services.AddAutoMapper(x => x.AddMaps(Assembly.GetExecutingAssembly()));

services.AddMock();

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(_ => { });
app.MapControllers();
app.UseSwagger();

var authenticationOptions = configuration.GetRequiredSection("Authentication").GetRequired<AuthenticationOptions>();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.OAuthClientId(authenticationOptions.ClientId);
    c.OAuthClientSecret(authenticationOptions.ClientSecret);
    c.OAuthUsePkce();
    c.OAuthScopes("openid", "profile");
});

app.Run();