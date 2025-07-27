using AccountService.Api.Domains.Enums;
using AccountService.Api.ViewModels;
using MediatR;

namespace AccountService.Api.Features.Transactions.ExecuteTransaction;

public class ExecuteTransactionCommand : IRequest<TransactionViewModel>
{
    public required Guid BankAccountId { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required TransactionType Type { get; set; }
    public required string Description { get; set; }
}
