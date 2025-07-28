using AccountService.Api.ViewModels;
using MediatR;

namespace AccountService.Api.Features.Account.GetAccounts;

public class GetAccountsQuery : IRequest<IEnumerable<AccountViewModel>>;