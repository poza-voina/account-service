using AccountService.Api.Exceptions;
using MediatR;

namespace AccountService.Api.Features.Account.CheckAccountExists;

public class CheckAccountQueryHandler(IAccountStorageService accountStorageService) : IRequestHandler<CheckAccountQuery, Unit>
{
    private const string NotFoundErrorMessage = "—чет не создан";

    public async Task<Unit> Handle(CheckAccountQuery request, CancellationToken cancellationToken)
    {
        if (await accountStorageService.CheckExistsAsync(request.Id, cancellationToken))
        {
            return Unit.Value;
        }

        throw new NotFoundException(NotFoundErrorMessage);
    }
}
