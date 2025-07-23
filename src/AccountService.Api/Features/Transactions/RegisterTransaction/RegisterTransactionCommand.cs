using AccountService.Api.Domains.Enums;

namespace AccountService.Api.Features.Transactions.RegisterTransaction;

public class RegisterTransactionCommand
{
    public Guid BankAccountId { get; set; }
    public Guid CounterpartyBankAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public TransactionType Type { get; set; }
    public string Description { get; set; }
}
