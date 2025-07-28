using AccountService.Api.Domains.Enums;

namespace AccountService.Api.Domains;

public class Account
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    public required string Currency { get; set; }
    public decimal Balance { get; set; }
    public decimal? InterestRate { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = [];
}
