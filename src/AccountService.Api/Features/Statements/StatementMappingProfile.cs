using AccountService.Api.ViewModels;
using AutoMapper;
using JetBrains.Annotations;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Features.Statements;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class StatementMappingProfile : Profile
{
    public StatementMappingProfile()
    {
        CreateMap<Models.Transaction, TransactionViewModel>();
        CreateMap<Models.Account, AccountWithTransactionsViewModel>();
    }
}
