using RabbitMQ.Client;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IRabbitMqConnectionManager
{
    // ReSharper disable once UnusedMember.Global Используется
    IConnection Connection { get; }
    IChannel PublishChannel { get; }
}
