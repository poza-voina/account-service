using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Api.Features.Statements.GetStatement;

public class GetStatementQueryHandler(
    IAccountStorageService accountStorageService,
    IClientVerificationService verificationService,
    IDatetimeHelper datetimeHelper,
    IMapper mapper) : IRequestHandler<GetStatementQuery, AccountWithTransactionsViewModel>
{
    public async Task<AccountWithTransactionsViewModel> Handle(GetStatementQuery request, CancellationToken cancellationToken)
    {
        await verificationService.VerifyAsync(request.OwnerId);
        (request.StartDateTime, request.EndDateTime) = datetimeHelper.NormalizeDateRange(request.StartDateTime, request.EndDateTime);

        var account = await accountStorageService.GetAccountAsync(
            request.AccountId,
            cancellationToken,
            x => x.Include(account => account.Transactions));

        account.Transactions = [.. account.Transactions.Where(
                x =>
                (request.StartDateTime == null || x.CreatedAt >= request.StartDateTime) &&
                (request.EndDateTime == null || x.CreatedAt <= request.EndDateTime))];

        return mapper.Map<AccountWithTransactionsViewModel>(account);
    }
}
