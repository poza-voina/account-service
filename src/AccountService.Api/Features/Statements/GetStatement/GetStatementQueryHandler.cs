using AccountService.Api.Features.Account;
using AccountService.Api.ObjectStorage;
using AccountService.Api.ViewModels;
using AutoMapper;
using MediatR;
using System.Reflection;

namespace AccountService.Api.Features.Statement.GetStatement;

public class GetStatementQueryHandler(
    IAccountStorageService accountStorageService,
    IClientVefiricationService vefiricationService,
    IDatetimeHelper datetimeHelper,
    IMapper mapper) : IRequestHandler<GetStatementQuery, AccountWithTransactionsViewModel>
{
    public async Task<AccountWithTransactionsViewModel> Handle(GetStatementQuery request, CancellationToken cancellationToken)
    {
        await vefiricationService.VerifyAsync(request.OwnerId);
        (request.StartDateTime, request.EndDateTime) = datetimeHelper.NormalizeDateRange(request.StartDateTime, request.EndDateTime);

        var account = await accountStorageService.GetAccountAsync(request.AccountId, cancellationToken);

        account.Transactions = account.Transactions.Where(
                x =>
                (request.StartDateTime == null || x.CreatedAt >= request.StartDateTime) &&
                (request.EndDateTime == null || x.CreatedAt <= request.EndDateTime))
            .ToList();

        return mapper.Map<AccountWithTransactionsViewModel>(account);
    }
}
