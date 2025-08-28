using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Transactions.Interfaces;
using AccountService.Api.ObjectStorage;
using AccountService.Api.ObjectStorage.Events.Published;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Features.Transactions.ApplyTransaction;

public class ApplyTransactionCommandHandler(
    IServiceProvider provider,
    IMediator mediator,
    ITransactionStorageService transactionStorageService,
    IEventFactory eventFactory)
    : UnitHandlerBase<ApplyTransactionCommand, Unit>(provider), IRequestHandler<ApplyTransactionCommand, Unit>
{
    private const string EnumErrorMessage = "Недопустимое значение перечисления";
    private const string NotEnoughMoneyErrorMessage = "На счете недостаточно средств";

    protected override async Task<Unit> ExecuteTransactionBodyAsync(ApplyTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionStorageService.GetTransactionAsync(
           request.AnyTransaction.TransactionId,
           cancellationToken,
           x => x.Include(transaction => transaction.BankAccount));

        var account = transaction.BankAccount!;

        account.Version = request.AnyTransaction.AccountVersion;

        switch (transaction.Type)
        {
            case TransactionType.Debit:
                ProcessDebit(transaction, account);
                await ProcessDebitEvent(transaction);
                break;

            case TransactionType.Credit:
                ProcessCredit(transaction, account);
                await ProcessCreditEvent(transaction);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(request), transaction.Type, EnumErrorMessage);
        }

        await transactionStorageService.ApplyTransactionAsync(transaction, account, cancellationToken);

        return Unit.Value;
    }

    private async Task ProcessCreditEvent(Transaction transaction)
    {
        var moneyCredited = new MoneyCredited
        {
            AccountId = transaction.BankAccountId,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            OperationId = transaction.Id
        };

        var @event = eventFactory.CreateEvent(moneyCredited, nameof(ApplyTransactionCommandHandler));

        await mediator.Publish(@event);
    }

    private async Task ProcessDebitEvent(Transaction transaction)
    {
        var moneyDebited = new MoneyDebited
        {
            AccountId = transaction.BankAccountId,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            OperationId = transaction.Id,
            Reason = transaction.Description
        };

        var @event = eventFactory.CreateEvent(moneyDebited, nameof(ApplyTransactionCommandHandler));

        await mediator.Publish(@event);
    }

    public static void ProcessDebit(Transaction transaction, Models.Account account)
    {
        account.Balance += transaction.Amount;
    }

    public static void ProcessCredit(Transaction transaction, Models.Account account)
    {
        if (transaction.Amount > account.Balance)
        {
            throw new UnprocessableException(NotEnoughMoneyErrorMessage);
        }

        account.Balance -= transaction.Amount;
    }
}
