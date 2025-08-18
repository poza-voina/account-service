using AccountService.Api.ObjectStorage.Interfaces;
using RabbitMQ.Client;

namespace AccountService.Api.ObjectStorage;

public class RabbitMqConnectionManager(IRabbitMqConnectionMonitor monitor) : IRabbitMqConnectionManager
{
    public IConnection Connection => monitor.Connection ?? throw new InvalidOperationException("Нет соединения с RabbitMq");
    public IChannel PublishChannel => monitor.PublishChannel ?? throw new InvalidOperationException("Нет соединения с RabbitMq");
}