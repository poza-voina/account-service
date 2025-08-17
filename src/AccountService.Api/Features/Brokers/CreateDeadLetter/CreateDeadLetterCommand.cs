using JetBrains.Annotations;
using MediatR;

namespace AccountService.Api.Features.Brokers.CreateDeadLetter;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CreateDeadLetterCommand : IRequest<Unit>
{
    public string? EventType { get; set; }
    public string? Payload { get; set; }
    public required string ExceptionMessage { get; set; }
    public string? StackTrace { get; set; }
}
