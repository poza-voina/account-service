using AccountService.Api.ViewModels;
using MediatR;

namespace AccountService.Api.Features.Account.PatchAccount;

public class PatchAccountCommand : IRequest<AccountViewModel>
{
    public required Guid Id { get; set; }
    public DateTime? ClosingDate { get; set; }
    public decimal? InterestRate { get; set; }
}