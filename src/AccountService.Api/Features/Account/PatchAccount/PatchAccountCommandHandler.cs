using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.ViewModels;
using AccountService.Infrastructure.Enums;
using AutoMapper;
using MediatR;

namespace AccountService.Api.Features.Account.PatchAccount;

public class PatchAccountCommandHandler(IAccountStorageService accountStorageService, IMapper mapper) : IRequestHandler<PatchAccountCommand, AccountViewModel>
{
    private const string AccountTypeErrorMessage = "Процентная ставка может быть только у депозита или кредита";

    public async Task<AccountViewModel> Handle(PatchAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await accountStorageService.GetAccountAsync(request.Id, cancellationToken);

        if (account.Type is not (AccountType.Deposit or AccountType.Credit))
        {
            throw new UnprocessableException(AccountTypeErrorMessage);
        }

        account.InterestRate = request.InterestRate;
        account.ClosingDate = request.ClosingDate;
        account.Version = request.Version;

        account = await accountStorageService.UpdateAccountAsync(account, cancellationToken);

        var result = mapper.Map<AccountViewModel>(account);

        return result;
    }
}
