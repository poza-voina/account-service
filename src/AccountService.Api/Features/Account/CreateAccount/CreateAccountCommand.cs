using AccountService.Api.Domains.Enums;

namespace AccountService.Api.Features.Account.CreateAccount;

public class CreateAccountCommand
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; }
    public decimal? InterestRate { get; set; }
    public DateTime? ClosingDate { get; set; }
}
