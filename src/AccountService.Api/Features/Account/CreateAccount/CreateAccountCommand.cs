using AccountService.Api.Domains.Enums;
using AccountService.Api.ViewModels;
using MediatR;

namespace AccountService.Api.Features.Account.CreateAccount;

public class CreateAccountCommand : IRequest<AccountViewModel>
{
    public required Guid OwnerId { get; set; }
    public required AccountType Type { get; set; }
    public required string Currency { get; set; }
    public decimal? InterestRate { get; set; }
    public DateTime? ClosingDate { get; set; }
}
