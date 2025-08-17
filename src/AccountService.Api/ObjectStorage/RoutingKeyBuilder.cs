namespace AccountService.Api.ObjectStorage;

public class RoutingKeyBuilder
{
    private readonly Dictionary<string, string> _map;

    public RoutingKeyBuilder(Dictionary<string, string> map)
    {
        _map = map;
    }

    public RoutingKeyBuilder Map<TPayload>(string routingKey)
    {
        _map[nameof(TPayload)] = routingKey;
        return this;
    }
}