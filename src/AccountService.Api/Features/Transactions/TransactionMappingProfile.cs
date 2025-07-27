using AccountService.Api.Features.Transactions.ExecuteTransaction;
using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.Features.Transactions.TransferTransaction;
using AutoMapper;

namespace AccountService.Api.Features.Transactions;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<RegisterTransactionCommand, Domains.Transaction>();
        CreateMap<TrasferTransactionCommand, RegisterTransactionCommand>();
        CreateMap<ExecuteTransactionCommand, RegisterTransactionCommand>();
    }
}
