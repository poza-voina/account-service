using AccountService.Api.Features.Transactions.ApplyTransaction;
using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.ViewModels;
using AutoMapper;
using MediatR;

namespace AccountService.Api.Features.Transactions.ExecuteTransaction;

public class ExecuteTransactionCommandHandler(IMediator mediator, IMapper mapper) : IRequestHandler<ExecuteTransactionCommand, TransactionViewModel>
{
    public async Task<TransactionViewModel> Handle(ExecuteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await mediator.Send(mapper.Map<RegisterTransactionCommand>(request), cancellationToken);
        await mediator.Send(new ApplyTransactionCommand { TransactionId = transaction.Id}, cancellationToken);

        return transaction;
    }
}
