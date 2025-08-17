using AccountService.Abstractions.Constants;
using AccountService.Api.ObjectStorage.Interfaces;

namespace AccountService.Api.ObjectStorage.Middlewares;

public class EventDispatchMiddleware(RequestDelegate next, ILogger<EventDispatchMiddleware> logger)
{
    // ReSharper disable once UnusedMember.Global Нужен используется для event
    public async Task InvokeAsync(HttpContext context)
    {
        if (HttpMethods.IsGet(context.Request.Method))
        {
            await next(context);
            return;
        }

        var dispatcher = context.RequestServices.GetRequiredService<IEventDispatcher>();
        var unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();

        if (context.Items.ContainsKey(SystemConstants.TransactionStartedKey))
        {
            logger.LogWarning($"Транзакция была начата до {nameof(EventDispatchMiddleware)}");
        }

        await unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, CancellationToken.None);

        context.Items[SystemConstants.TransactionStartedKey] = true;

        try
        {
            await next(context);

            await dispatcher.DispatchAllAsync(CancellationToken.None);

            await unitOfWork.CommitAsync(CancellationToken.None);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(CancellationToken.None);

            throw;
        }
    }
}
