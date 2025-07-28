using AccountService.Api.Features.Transactions.ExecuteTransaction;
using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.Features.Transactions.TransferTransaction;
using AutoMapper;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Transactions;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<RegisterTransactionCommand, Domains.Transaction>();
        CreateMap<TransferTransactionCommand, RegisterTransactionCommand>();
        CreateMap<ExecuteTransactionCommand, RegisterTransactionCommand>();
    }
}
