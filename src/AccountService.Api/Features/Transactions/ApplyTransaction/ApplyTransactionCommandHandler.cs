using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.Features.Transactions.Interfaces;
using Models = AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Api.Features.Transactions.ApplyTransaction;

public class ApplyTransactionCommandHandler(ITransactionStorageService transactionStorageService) : IRequestHandler<ApplyTransactionCommand, Unit>
{
    private const string EnumErrorMessage = "Недопустимое значение перечисления";
    private const string NotEnoughMoneyErrorMessage = "На счете недостаточно средств";

    public async Task<Unit> Handle(ApplyTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionStorageService.GetTransactionAsync(
            request.AnyTransaction.TransactionId,
            cancellationToken,
            x => x.Include(x => x.BankAccount));

        var account = transaction.BankAccount!;

        account.Version = request.AnyTransaction.AccountVersion;

        switch (transaction.Type)
        {
            case TransactionType.Debit:
                ProcessDebit(transaction, account);
                break;

            case TransactionType.Credit:
                ProcessCredit(transaction, account);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(request), transaction.Type, EnumErrorMessage);
        }

        await transactionStorageService.ApplyTransactionAsync(transaction, account, cancellationToken);

        return Unit.Value;
    }

    public static void ProcessDebit(Models.Transaction transaction, Models.Account account)
    {
        account.Balance += transaction.Amount;
    }

    public static void ProcessCredit(Models.Transaction transaction, Models.Account account)
    {
        if (transaction.Amount > account.Balance)
        {
            throw new UnprocessableException(NotEnoughMoneyErrorMessage);
        }

        account.Balance -= transaction.Amount;
    }
}
