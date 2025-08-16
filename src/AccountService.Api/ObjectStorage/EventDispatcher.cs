using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using System.Text.Json;

namespace AccountService.Api.ObjectStorage;

public class EventDispatcher(IEventCollector eventCollector, IRepository<OutboxMessage> repository) : IEventDispatcher
{
    public async Task DispatchAllAsync(CancellationToken cancellationToken)
    {
        var events = eventCollector.GetEvents();

        if (!events.Any())
        {
            return;
        }

        var outboxMessages = events.Select(x => new OutboxMessage
        {
            EventType = x.GetPayloadType().FullName ?? throw new InvalidOperationException("Не удалось найти тип payload"), // TODO придумать что-то с ошибкой
            EventPayload = JsonSerializer.Serialize(x),
            Status = OutboxStatus.Pending
        }).ToList();


        await repository.AddRangeAsync(outboxMessages, cancellationToken);
    }
}
