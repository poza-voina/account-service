using MediatR;

namespace AccountService.Api.Features.Transactions.ApplyTransactionPair;

public class ApplyTransactionPairCommand : IRequest<Unit>
{
    /// <summary>
    /// Идентификатор первого счета
    /// </summary>
    public required Guid FirstTransactionId { get; set; }

    /// <summary>
    /// Идентификатор второго счета
    /// </summary>
    public required Guid SecondTransactionId { get; set; }
}
