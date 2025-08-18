using MediatR;

namespace AccountService.Api.Features.Brokers.CreateInboxConsumed;

public class CreateInboxConsumedCommand : IRequest<Unit>
{
    public Guid MessageId { get; set; }
    public required string Handler { get; set; }
}
