using MediatR;

namespace AccountService.Api.Features.Account.BlockAccounts;

public class BlockAccountsCommand : IRequest<Unit>
{
    public required Guid OwnerId { get; set; }
}