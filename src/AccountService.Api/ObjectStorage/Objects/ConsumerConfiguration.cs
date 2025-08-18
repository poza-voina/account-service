namespace AccountService.Api.ObjectStorage.Objects;

public class ConsumerConfiguration
{
    public string EventType { get; private set; } = "";
    public string QueueName { get; private set; } = "";

    public ConsumerConfiguration Map<T>()
    {
        EventType = typeof(T).Name;
        
        return this;
    }

    public ConsumerConfiguration WithQueueName(string queueName)
    {
        QueueName = queueName;

        return this;
    }
}
