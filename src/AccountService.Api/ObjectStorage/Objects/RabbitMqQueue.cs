namespace AccountService.Api.ObjectStorage.Objects;

public class RabbitMqQueue
{
    public required string Name { get; set; }
    public required string RoutingKey { get; set; }
}