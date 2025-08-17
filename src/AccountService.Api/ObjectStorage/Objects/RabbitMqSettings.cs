namespace AccountService.Api.ObjectStorage.Objects;

public class RabbitMqSettings
{
    public required string HostName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Exchange { get; set; }
    public List<RabbitMqQueue> Queues { get; set; } = [];
}