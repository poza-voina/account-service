using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Transactions.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Infrastructure.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Api.Features.Transactions.ApplyTransactionPair;

public class ApplyTransactionPairCommandHandler(ITransactionStorageService transactionStorageService) : IRequestHandler<ApplyTransactionPairCommand, Unit>
{
    private const string InvalidTransactionLinkErrorMessage = "Транзакции не формируют взаимную связь";
    private const string AccountNotFoundMoney = "На счете недостаточно средств";
    private const string CreditTransactionNotFoundErrorMessage = "Транзакция на списание не найдена";
    private const string DebitTransactionNotFoundErrorMessage = "Транзакция на пополнение не найдена";

    public async Task<Unit> Handle(ApplyTransactionPairCommand request, CancellationToken cancellationToken)
    {
        var transactions = await transactionStorageService.GetTransactionsAsync(
            [request.CreditTransaction.TransactionId, request.DebitTransaction.TransactionId],
            cancellationToken,
            x => x.Include(transaction => transaction.BankAccount));

        var transactionsList = transactions.ToList();
        var credit = request.CreditTransaction.WithTransaction(
            transactionsList.FirstOrDefault(x => x.Type == TransactionType.Credit)
            ?? throw new NotFoundException(CreditTransactionNotFoundErrorMessage));

        var debit = request.DebitTransaction.WithTransaction(
            transactionsList.FirstOrDefault(x => x.Type == TransactionType.Debit)
            ?? throw new NotFoundException(DebitTransactionNotFoundErrorMessage));


        if (credit.Transaction.BankAccountId == debit.Transaction.CounterpartyBankAccountId &&
            credit.Transaction.CounterpartyBankAccountId == debit.Transaction.BankAccountId &&
            credit.Transaction.CounterpartyBankAccountId is not null)
        {
            throw new UnprocessableException(InvalidTransactionLinkErrorMessage);
        }

        ChangeVersion(credit, debit);
        ProcessCredit(credit);
        ProcessDebit(debit);

        await transactionStorageService.ApplyTransactionsAsync(
            [credit.Transaction, debit.Transaction],
            [credit.Transaction.BankAccount!, debit.Transaction.BankAccount!],
            cancellationToken);

        return Unit.Value;
    }

    private static void ChangeVersion(TransactionInfo credit, TransactionInfo debit)
    {
        credit.Transaction.BankAccount!.Version = credit.AccountVersion;
        debit.Transaction.BankAccount!.Version = debit.AccountVersion;
    }

    private static void ProcessDebit(TransactionInfo debit)
    {
        debit.Transaction.BankAccount!.Balance += debit.Transaction.Amount;
    }

    private static void ProcessCredit(TransactionInfo credit)
    {
        if (credit.Transaction.BankAccount!.Balance < credit.Transaction.Amount)
        {
            throw new UnprocessableException(AccountNotFoundMoney);
        }

        credit.Transaction.BankAccount.Balance -= credit.Transaction.Amount;
    }
}
