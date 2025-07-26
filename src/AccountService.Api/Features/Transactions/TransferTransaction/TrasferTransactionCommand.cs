using AccountService.Api.Domains.Enums;

namespace AccountService.Api.Features.Transactions.TransferTransaction;

public class TrasferTransactionCommand
{
    public required Guid BankAccountId { get; set; }
    public required Guid CounterpartyBankAccountId { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required TransactionType Type { get; set; }
    public required string Description { get; set; }
}

