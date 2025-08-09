using AccountService.Infrastructure.Enums;
using System.Transactions;

namespace AccountService.Infrastructure.Models;

// TODO: решить вопрос нужно ли это делить с доменными моделями или нет
public class AccountModel
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
