using AccountService.Api.Domains;
using AccountService.Api.Domains.Enums;
using AccountService.Api.Exceptions;
using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.Features.Transactions.Interfaces;
using MediatR;

namespace AccountService.Api.Features.Transactions.ApplyTransaction;

public class ApplyTransactionCommandHandler(ITransactionStorageService transactionStorageService, IAccountStorageService accountStorageService) : IRequestHandler<ApplyTransactionCommand, Unit>
{
    private const string EnumErrorMessage = "Недопустимое значение перечисления";
    private const string NotEnoughMoneyErrorMessage = "На счете недостаточно средств";

    public async Task<Unit> Handle(ApplyTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionStorageService.GetTransactionAsync(request.TransactionId, cancellationToken);

        var account = await accountStorageService.GetAccountAsync(transaction.BankAccountId, cancellationToken);

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

    public static void ProcessDebit(Transaction transaction, Domains.Account account)
    {
        transaction.IsApply = true;
        account.Balance += transaction.Amount;
    }

    public static void ProcessCredit(Transaction transaction, Domains.Account account)
    {
        if (transaction.Amount > account.Balance)
        {
            throw new UnprocessableException(NotEnoughMoneyErrorMessage);
        }
        transaction.IsApply = true;

        account.Balance -= transaction.Amount;
    }
}
