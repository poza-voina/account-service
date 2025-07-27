using MediatR;

namespace AccountService.Api.Features.Transactions.ApplyTransaction;

public class ApplyTransactionCommand : IRequest<Unit>
{
    /// <summary>
    /// Идентификатор транзакции
    /// </summary>
    public required Guid TransactionId { get; set; }
}
