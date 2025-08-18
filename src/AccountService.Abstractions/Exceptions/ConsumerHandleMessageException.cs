namespace AccountService.Abstractions.Exceptions;

public class ConsumerHandleMessageException : Exception
{
    public string? EventType { get; set; }
    public string Payload { get; set; }

    public ConsumerHandleMessageException(string? eventType, string payload, string? message, Exception? innerException) : base(message, innerException)
    {
        EventType = eventType;
        Payload = payload;
    }

    public ConsumerHandleMessageException(string payload, string message) : base(message)
    {
        Payload = payload;
    }
}
