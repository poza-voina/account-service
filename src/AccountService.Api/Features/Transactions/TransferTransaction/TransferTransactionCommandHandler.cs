using AccountService.Abstractions.Constants;
using AccountService.Api.Features.Transactions.ApplyTransactionPair;
using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.ObjectStorage.Events.Published;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Api.ViewModels;
using AccountService.Infrastructure.Enums;
using AutoMapper;
using MediatR;
using System.Data;

namespace AccountService.Api.Features.Transactions.TransferTransaction;

public class TransferTransactionCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IMapper mapper,
    IEventFactory eventFactory)
    : IRequestHandler<TransferTransactionCommand, TransferTransactionViewModel>
{
    public async Task<TransferTransactionViewModel> Handle(TransferTransactionCommand request, CancellationToken cancellationToken)
    {
        TransferTransactionViewModel result;
        if (IsTransactionStarted)
        {
            result = await ExecuteTransactionBody(request, cancellationToken);
        }
        else
        {
            result = await ExecuteTransaction(request, cancellationToken);
        }

        var transferCompleted = new TransferCompleted
        {
            SourceAccountId = request.BankAccountId,
            DestinationAccountId = request.CounterpartyBankAccountId,
            Amount = request.Amount,
            Currency = request.Currency,
            TransferId = Guid.NewGuid() // NOTE ????
        };

        var @event = eventFactory.CreateEvent(transferCompleted, nameof(TransferTransactionCommandHandler));

        await mediator.Publish(@event, cancellationToken);

        return result;
    }

    private async Task<TransferTransactionViewModel> ExecuteTransaction(TransferTransactionCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        try
        {
            var result = await ExecuteTransactionBody(request, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<TransferTransactionViewModel> ExecuteTransactionBody(TransferTransactionCommand request, CancellationToken cancellationToken)
    {
        var credit = await mediator.Send(BuildCreditTransaction(request), cancellationToken);
        var debit = await mediator.Send(BuildDebitTransaction(request), cancellationToken);

        var applyTransactionsCommand = new ApplyTransactionPairCommand
        {
            CreditTransaction = new TransactionInfo
            {
                TransactionId = credit.Id,
                AccountVersion = request.BankAccountVersion
            },
            DebitTransaction = new TransactionInfo
            {
                TransactionId = debit.Id,
                AccountVersion = request.CounterpartyBankAccountVersion
            }
        };

        await mediator.Send(applyTransactionsCommand, cancellationToken);

        return new TransferTransactionViewModel { Credit = credit, Debit = debit };
    }

    private RegisterTransactionCommand BuildCreditTransaction(TransferTransactionCommand request)
    {
        var registerCommand = mapper.Map<RegisterTransactionCommand>(request);
        registerCommand.Type = TransactionType.Credit;

        return registerCommand;
    }

    private RegisterTransactionCommand BuildDebitTransaction(TransferTransactionCommand request)
    {
        var registerCommand = mapper.Map<RegisterTransactionCommand>(request);
        registerCommand.BankAccountId = request.CounterpartyBankAccountId;
        registerCommand.CounterpartyBankAccountId = request.CounterpartyBankAccountId;
        registerCommand.Type = TransactionType.Debit;

        return registerCommand;
    }

    private bool IsTransactionStarted =>
        httpContextAccessor.HttpContext
        ?.Items
        .ContainsKey(SystemConstants.TransactionStartedKey) is true;
}
