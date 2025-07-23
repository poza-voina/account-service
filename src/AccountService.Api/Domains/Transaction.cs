using AccountService.Api.Domains.Enums;

namespace AccountService.Api.Domains;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid BankAccountId { get; set; }
    public Guid CounterpartyBankAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public TransactionType Type { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
