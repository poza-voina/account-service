using AccountService.Infrastructure.Repositories.Interfaces;
using Models = AccountService.Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AccountService.Api.ObjectStorage.Interfaces;
using System.Data;
using AccountService.Api.ObjectStorage;

namespace AccountService.Api.Features.Account.BlockAccounts;

public class BlockAccountsCommandHandler(
    IRepository<Models.Account> repository,
    IServiceProvider provider)
    : UnitHandlerBase<BlockAccountsCommand, Unit>(provider)
{
    protected override async Task<Unit> ExecuteTransactionBodyAsync(BlockAccountsCommand request, CancellationToken cancellationToken)
    {
        var accounts = await repository.GetAll()
            .Where(x => x.OwnerId == request.OwnerId)
            .ToListAsync(cancellationToken: cancellationToken);

        accounts.ForEach(x => x.IsFrozen = true);

        await repository.UpdateRangeAsync(accounts);

        return Unit.Value;
    }
}