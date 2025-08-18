using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.Scheduler.Interfaces;
using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Scheduler.Jobs;

public class AccrueInterestJob(IUnitOfWork unitOfWork) : IJob
{
    public async Task Execute()
    {
        var repository = unitOfWork.GetRepository<IRepository<Models.Account>>();

        await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, CancellationToken.None);

        try
        {
            var ids = await repository.GetAll()
                .Where(x => x.Type == AccountType.Deposit)
                .Select(x => x.Id)
                .ToListAsync();

            foreach(var item in ids)
            {
                await repository.ExecuteSqlAsync($"CALL accrue_interest({item})");
            }

            await unitOfWork.CommitAsync(CancellationToken.None);
        } 
        catch
        {
            await unitOfWork.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}
