using AccountService.Abstractions.Constants;
using AccountService.Api.ObjectStorage.Interfaces;

namespace AccountService.Api.ObjectStorage.Middlewares;

public class EventDispatchMiddleware(RequestDelegate next, ILogger<EventDispatchMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (HttpMethods.IsGet(context.Request.Method))
        {
            await next(context);
            return;
        }

        var dispatcher = context.RequestServices.GetRequiredService<IEventDispatcher>();
        var unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();

        if (context.Items.ContainsKey(SystemConstatns.TRANSACTION_STARTED_KEY))
        {
            logger.LogWarning($"Транзакция была начата до {nameof(EventDispatchMiddleware)}");
        }

        await unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, CancellationToken.None); //TODO что-то с canellationToken

        context.Items[SystemConstatns.TRANSACTION_STARTED_KEY] = true;

        try
        {
            await next(context);

            await dispatcher.DispatchAllAsync(CancellationToken.None);

            await unitOfWork.CommitAsync(CancellationToken.None); //TODO что-то с canellationToken
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(CancellationToken.None); //TODO что-то с canellationToken

            throw;
        }
    }
}
