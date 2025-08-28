using AccountService.Abstractions.Constants;
using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Transactions.ApplyTransaction;
using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.ObjectStorage;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Api.ViewModels;
using AutoMapper;
using MediatR;

namespace AccountService.Api.Features.Transactions.ExecuteTransaction;

public class ExecuteTransactionCommandHandler(
    IServiceProvider provider,
    IMediator mediator,
    IMapper mapper)
    : UnitHandlerBase<ExecuteTransactionCommand, TransactionViewModel>(provider),
    IRequestHandler<ExecuteTransactionCommand, TransactionViewModel>
{
    protected override async Task<TransactionViewModel> ExecuteTransactionBodyAsync(ExecuteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await mediator.Send(mapper.Map<RegisterTransactionCommand>(request), cancellationToken);

        await mediator.Send(
            new ApplyTransactionCommand
            {
                AnyTransaction = new TransactionInfo
                {
                    TransactionId = transaction.Id,
                    AccountVersion = request.BankAccountVersion
                }
            },
            cancellationToken);

        return transaction;
    }
}
