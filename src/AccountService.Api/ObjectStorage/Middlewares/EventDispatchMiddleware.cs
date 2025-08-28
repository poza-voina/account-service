using AccountService.Abstractions.Constants;
using AccountService.Api.ObjectStorage.Interfaces;

namespace AccountService.Api.ObjectStorage.Middlewares;

public class EventDispatchMiddleware(RequestDelegate next)
{
    // ReSharper disable once UnusedMember.Global Нужен используется для event
    public async Task InvokeAsync(HttpContext context)
    {
        var dispatcher = context.RequestServices.GetRequiredService<IEventDispatcher>();

        await next(context);

        await dispatcher.DispatchAllAsync(CancellationToken.None);
    }
}
