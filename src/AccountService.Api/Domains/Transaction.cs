using AccountService.Api.Domains.Enums;

namespace AccountService.Api.Domains;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid BankAccountId { get; set; }
    public Guid CounterpartyBankAccountId { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public TransactionType Type { get; set; }
    public required string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
