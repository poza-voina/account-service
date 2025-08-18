using MediatR;

namespace AccountService.Api.Features.Account.UnblockAccounts;

public class UnblockAccountsCommand : IRequest<Unit>
{
    public required Guid OwnerId { get; set; }
}
