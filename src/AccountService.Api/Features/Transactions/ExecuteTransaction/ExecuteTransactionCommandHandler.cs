using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Transactions.ApplyTransaction;
using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ViewModels;
using AutoMapper;
using MediatR;
using System.Data;

namespace AccountService.Api.Features.Transactions.ExecuteTransaction;

public class ExecuteTransactionCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper) : IRequestHandler<ExecuteTransactionCommand, TransactionViewModel>
{
    public async Task<TransactionViewModel> Handle(ExecuteTransactionCommand request, CancellationToken cancellationToken)
    {
        using var _ = await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        try
        {
            var transaction = await mediator.Send(mapper.Map<RegisterTransactionCommand>(request), cancellationToken);
            await mediator.Send(new ApplyTransactionCommand { TransactionId = transaction.Id }, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return transaction;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw new UnprocessableException("Не удалось перенести деньги", ex);
        }
    }
}
