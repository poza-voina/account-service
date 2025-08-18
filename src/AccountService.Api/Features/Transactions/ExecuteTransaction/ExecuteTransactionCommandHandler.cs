using AccountService.Abstractions.Constants;
using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Transactions.ApplyTransaction;
using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Api.ViewModels;
using AutoMapper;
using MediatR;
using System.Data;

namespace AccountService.Api.Features.Transactions.ExecuteTransaction;

public class ExecuteTransactionCommandHandler(
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<ExecuteTransactionCommand, TransactionViewModel>
{
    public async Task<TransactionViewModel> Handle(ExecuteTransactionCommand request, CancellationToken cancellationToken)
    {
        if (IsTransactionStarted)
        {
            return await ExecuteTransactionBody(request, cancellationToken);
        }

        return await ExecuteTransaction(request, cancellationToken);
    }

    private async Task<TransactionViewModel> ExecuteTransaction(ExecuteTransactionCommand request, CancellationToken cancellationToken)
    {
        await using var _ = await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        try
        {
            var result = await ExecuteTransactionBody(request, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw new UnprocessableException("Не удалось перенести деньги", ex);
        }
    }

    private async Task<TransactionViewModel> ExecuteTransactionBody(ExecuteTransactionCommand request, CancellationToken cancellationToken)
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

    private bool IsTransactionStarted =>
        httpContextAccessor.HttpContext
        ?.Items
        .ContainsKey(SystemConstants.TransactionStartedKey) is true;
}
