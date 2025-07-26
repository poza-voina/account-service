using AccountService.Api.ViewModels;
using AutoMapper;

namespace AccountService.Api.Features.Statements;

public class StatmentMappingProfile : Profile
{
    public StatmentMappingProfile()
    {
        CreateMap<Domains.Transaction, TransactionViewModel>();
        CreateMap<Domains.Account, AccountWithTransactionsViewModel>();
    }
}
