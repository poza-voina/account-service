using AccountService.Api.ViewModels;
using AutoMapper;
using MediatR;

namespace AccountService.Api.Features.Account.GetAccounts;

public class GetAccountsQueryHandler(IAccountStorageService accountStorageService, IMapper mapper) : IRequestHandler<GetAccountsQuery, IEnumerable<AccountViewModel>>
{
    public async Task<IEnumerable<AccountViewModel>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await accountStorageService.GetAccountsAsync(cancellationToken);

        var result = mapper.Map<IEnumerable<AccountViewModel>>(accounts);

        return result;
    }
}
