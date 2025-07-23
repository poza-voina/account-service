using AccountService.Api.Domains.Enums;

namespace AccountService.Api.Features.Transactions.TransferTransaction;

public class TrasferTransactionCommand
{
    public Guid BankAccountId { get; set; }
    public Guid CounterpartyBankAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public TransactionType Type { get; set; }
    public string Description { get; set; }
}

