using AccountService.Infrastructure.Repositories.Interfaces;
using Models = AccountService.Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AccountService.Api.ObjectStorage.Interfaces;
using System.Data;

namespace AccountService.Api.Features.Account.BlockAccounts;

public class BlockAccountsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<BlockAccountsCommand, Unit>
{
    public async Task<Unit> Handle(BlockAccountsCommand request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<IRepository<Models.Account>>();

        await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        try
        {
            var accounts = await repository.GetAll()
                .Where(x => x.OwnerId == request.OwnerId)
                .ToListAsync();

            accounts.ForEach(x => x.IsFrozen = true);

            await repository.UpdateRangeAsync(accounts);

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