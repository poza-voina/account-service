using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Api.ObjectStorage.Events;
using AccountService.Api.ObjectStorage.Events.Published;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.Scheduler.Interfaces;
using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Scheduler.Jobs;

public class AccrueInterestJob(
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IEventFactory eventFactory,
    IEventDispatcher eventDispatcher)
    : IJob
{
    public async Task Execute()
    {
        var repository = unitOfWork.GetRepository<IRepository<Models.Account>>();

        await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, CancellationToken.None);

        try
        {
            var accounts = await repository.GetAll()
                .Where(x => x.Type == AccountType.Deposit)
                .Select(x => new { x.Id, x.InterestRate })
                .ToListAsync();

            foreach(var item in accounts)
            {
                var interestAccrued = new InterestAccrued
                {
                    AccountId = item.Id,
                    PeriodFrom = DateTime.UtcNow.AddDays(-1),
                    PeriodTo = DateTime.UtcNow,
                    Amount = item.InterestRate!.Value // NOTE: ???
                };

                var @event = eventFactory.CreateEvent(interestAccrued, nameof(AccrueInterestJob));

                await mediator.Publish(@event);

                await repository.ExecuteSqlAsync($"CALL accrue_interest({item.Id})");
            }

            await eventDispatcher.DispatchAllAsync(CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);
        } 
        catch
        {
            await unitOfWork.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}
