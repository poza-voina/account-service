using AccountService.Api.ViewModels;
using AutoMapper;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Statements;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class StatementMappingProfile : Profile
{
    public StatementMappingProfile()
    {
        CreateMap<Domains.Transaction, TransactionViewModel>();
        CreateMap<Domains.Account, AccountWithTransactionsViewModel>();
    }
}
