using AccountService.Infrastructure.Enums;

namespace AccountService.Infrastructure.Models;

public class Account : IDatabaseModel, IConcurrencyModel
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength AccountConfiguration
    public required string Currency { get; set; }
    public decimal Balance { get; set; }
    public decimal? InterestRate { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; } = [];
    public virtual ICollection<Transaction> CounterPartyTransactions { get; set; } = [];
    public uint Version { get; set; }
}
