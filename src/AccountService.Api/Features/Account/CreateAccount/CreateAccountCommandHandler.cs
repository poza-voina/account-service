using AccountService.Abstractions.Constants;
using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.Features.Transactions.ExecuteTransaction;
using AccountService.Api.ObjectStorage.Events;
using AccountService.Api.ObjectStorage.Events.Published;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ViewModels;
using AccountService.Infrastructure.Models;
using AutoMapper;
using MediatR;
using Newtonsoft.Json.Bson;
using System.Threading;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Features.Account.CreateAccount;

public class CreateAccountCommandHandler(
    IMediator mediator,
    IUnitOfWork unitOfWork,
    IAccountStorageService accountStorageService,
    IMapper mapper,
    IClientVerificationService clientVerificationService,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<CreateAccountCommand, AccountViewModel>
{
    public async Task<AccountViewModel> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        await clientVerificationService.VerifyAsync(request.OwnerId);

        var account = mapper.Map<Models.Account>(request);

        account.Id = Guid.NewGuid();
        account.IsDeleted = false;

        if (IsTransactionStarted)
        {
            return await ExecuteTransactionBodyAsync(account, cancellationToken);
        }
        {
            return await ExecuteTransactionAsync(account, cancellationToken);
        }
    }

    private async Task<AccountViewModel> ExecuteTransactionAsync(Models.Account account, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken);

        try
        {
            return await ExecuteTransactionBodyAsync(account, cancellationToken);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<AccountViewModel> ExecuteTransactionBodyAsync(Models.Account account, CancellationToken cancellationToken)
    {
        account = await accountStorageService.CreateAccountAsync(account, cancellationToken);

        var result = mapper.Map<AccountViewModel>(account);

        var e = new Event<AccountOpened>
        {
            EventId = Guid.NewGuid(),
            OccuratedAt = DateTime.UtcNow.ToString(),
            Meta = new EventMeta
            {
                Version = "v1",
                CausationId = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid(),
                Source = nameof(CreateAccountCommandHandler)
            },
            Data = mapper.Map<AccountOpened>(account)
        };

        await mediator.Publish(e, cancellationToken);

        return result;
    }

    private bool IsTransactionStarted =>
        httpContextAccessor.HttpContext
        ?.Items
        .ContainsKey(SystemConstatns.TRANSACTION_STARTED_KEY) is true;
}