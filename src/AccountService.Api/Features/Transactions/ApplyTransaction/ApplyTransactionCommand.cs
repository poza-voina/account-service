using MediatR;

namespace AccountService.Api.Features.Transactions.ApplyTransaction;

public class ApplyTransactionCommand : IRequest<Unit>
{
    public required Guid TransactionId { get; set; }
}
