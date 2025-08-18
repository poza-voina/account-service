using AccountService.Api.ObjectStorage.Events;
using RabbitMQ.Client;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IRabbitMqConnectionManager
{
    IConnection Connection { get; }
    IChannel PublishChannel { get; }
}
