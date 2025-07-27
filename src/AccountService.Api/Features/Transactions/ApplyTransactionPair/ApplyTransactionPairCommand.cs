using MediatR;

namespace AccountService.Api.Features.Transactions.ApplyTransactionPair;

public class ApplyTransactionPairCommand : IRequest<Unit>
{
    public required Guid FirstTransactionId { get; set; }
    public required Guid SecondTransactionId { get; set; }
}
