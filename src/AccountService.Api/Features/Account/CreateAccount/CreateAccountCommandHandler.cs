using AccountService.Abstractions.Constants;
using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.ObjectStorage;
using AccountService.Api.ObjectStorage.Events.Published;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ViewModels;
using AutoMapper;
using MediatR;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Features.Account.CreateAccount;

public class CreateAccountCommandHandler(
    IServiceProvider provider,
    IMediator mediator,
    IAccountStorageService accountStorageService,
    IMapper mapper,
    IClientVerificationService clientVerificationService,
    IEventFactory eventFactory
    ) : UnitHandlerBase<CreateAccountCommand, AccountViewModel>(provider)
{
    protected override async Task<AccountViewModel> ExecuteTransactionBodyAsync(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        await clientVerificationService.VerifyAsync(request.OwnerId);

        var account = mapper.Map<Models.Account>(request);

        account.Id = Guid.NewGuid();
        account.IsDeleted = false;
        account.IsFrozen = false;

        account = await accountStorageService.CreateAccountAsync(account, cancellationToken);

        var result = mapper.Map<AccountViewModel>(account);

        var @event = eventFactory.CreateEvent(mapper.Map<AccountOpened>(account), nameof(CreateAccountCommandHandler));

        await mediator.Publish(@event, cancellationToken);

        return result;
    }
}