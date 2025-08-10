using AccountService.Infrastructure.Enums;

namespace AccountService.Infrastructure.Models;

public class Transaction : IDatabaseModel
{
    public required Guid Id { get; set; }
    public required Guid BankAccountId { get; set; }
    public Guid? CounterpartyBankAccountId { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required TransactionType Type { get; set; }
    public required string Description { get; set; }
    public required bool IsApply { get; set; }
    public required DateTime CreatedAt { get; set; }
    public virtual Account? BankAccount { get; set; }
    public virtual Account? CounterpartyBankAccount { get; set; }
}