using RabbitMQ.Client;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IRabbitMqConnectionMonitor
{
    public IConnection? Connection { get; }
    public IChannel? PublishChannel { get; }
}
