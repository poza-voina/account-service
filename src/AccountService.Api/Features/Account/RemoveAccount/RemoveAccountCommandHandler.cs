using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.ObjectStorage;
using MediatR;

namespace AccountService.Api.Features.Account.RemoveAccount;

public class RemoveAccountCommandHandler(IServiceProvider provider, IAccountStorageService accountStorageService) : UnitHandlerBase<RemoveAccountCommand, Unit>(provider)
{
    protected override async Task<Unit> ExecuteTransactionBodyAsync(RemoveAccountCommand request, CancellationToken cancellationToken)
    {
        await accountStorageService.RemoveAccountAsync(request.Id, cancellationToken);

        return Unit.Value;
    }
}
