using AccountService.Infrastructure.Repositories.Interfaces;
using Models = AccountService.Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AccountService.Api.ObjectStorage.Interfaces;
using System.Data;
using AccountService.Api.ObjectStorage;

namespace AccountService.Api.Features.Account.UnblockAccounts;

public class UnblockAccountsCommandHandler(IRepository<Models.Account> repository, IServiceProvider provider) : UnitHandlerBase<UnblockAccountsCommand, Unit>(provider)
{
    protected override async Task<Unit> ExecuteTransactionBodyAsync(UnblockAccountsCommand request, CancellationToken cancellationToken)
    {
        var accounts = await repository.GetAll()
            .Where(x => x.OwnerId == request.OwnerId && x.IsFrozen)
            .ToListAsync(cancellationToken: cancellationToken);

        accounts.ForEach(x => x.IsFrozen = false);

        await repository.UpdateRangeAsync(accounts);

        return Unit.Value;
    }
}
