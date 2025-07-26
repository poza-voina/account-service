using AccountService.Api;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllerConfiguration();

services.AddEndpointsApiExplorer();

services.AddSwaggerGenConfiguration();

services.AddValidationConfiguration();

services.AddMediatrConfiguration();

services.AddHelpers();

services.AddServices();

services.AddAutoMapper(x => x.AddMaps(Assembly.GetExecutingAssembly()));

services.AddMock();

services.AddProblemsConfiguration();

var app = builder.Build();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.Run();