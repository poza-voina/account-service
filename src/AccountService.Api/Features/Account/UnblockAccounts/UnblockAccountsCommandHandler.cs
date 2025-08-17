using AccountService.Infrastructure.Repositories.Interfaces;
using Models = AccountService.Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AccountService.Api.ObjectStorage.Interfaces;
using System.Data;

namespace AccountService.Api.Features.Account.UnblockAccounts;

public class UnblockAccountsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UnblockAccountsCommand, Unit>
{
    public async Task<Unit> Handle(UnblockAccountsCommand request, CancellationToken cancellationToken)
    {
        var respository = unitOfWork.GetRepository<IRepository<Models.Account>>();

        await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        try
        {
            var accounts = await respository.GetAll()
                .Where(x => x.OwnerId == request.OwnerId && x.IsFrozen)
                .ToListAsync();

            accounts.ForEach(x => x.IsFrozen = false);

            await respository.UpdateRangeAsync(accounts);

            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        return Unit.Value;
    }
}
