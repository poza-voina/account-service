using AccountService.Api.ObjectStorage.Objects;
using MediatR;

namespace AccountService.Api.Features.Transactions.ApplyTransactionPair;

public class ApplyTransactionPairCommand : IRequest<Unit>
{
    public required TransactionInfo CreditTransaction { get; set; }
    public required TransactionInfo DebitTransaction { get; set; }
}