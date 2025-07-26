using MediatR;

namespace AccountService.Api.Features.Account.RemoveAccount;

public class RemoveAccountCommandHandler(IAccountStorageService accountStorageService) : IRequestHandler<RemoveAccountCommand, Unit>
{
    public async Task<Unit> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
    {
        await accountStorageService.RemoveAccountAsync(request.Id, cancellationToken);

        return Unit.Value;
    }
}
