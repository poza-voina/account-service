using AccountService.Api.Domains.Enums;

namespace AccountService.Api.ViewModels;

public class TransactionViewModel
{
    public required Guid Id { get; set; }
    public required Guid BankAccountId { get; set; }
    public Guid? CounterpartyBankAccountId { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required TransactionType Type { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; }
}
