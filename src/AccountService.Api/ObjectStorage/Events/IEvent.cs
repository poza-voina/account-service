using System.Diagnostics.CodeAnalysis;

namespace AccountService.Api.ObjectStorage.Events;

public interface IEvent<out TPayload> : IEventBase
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")] 
    TPayload? Payload { get; }
}
