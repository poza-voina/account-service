using AccountService.Api.Domains;
using AccountService.Api.Domains.Enums;
using AccountService.Api.Exceptions;
using AccountService.Api.Features.Account;
using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.Features.Transactions.Interfaces;
using MediatR;

namespace AccountService.Api.Features.Transactions.ApplyTransactionPair;

public class ApplyTransactionPairCommandHandler(IAccountStorageService accountStorageService, ITransactionStorageService transactionStorageService) : IRequestHandler<ApplyTransactionPairCommand, Unit>
{
    private const string InvalidTransactionLinkErrorMessage = "Транзакции не формируют взаимную связь";
    private const string AccountNotFoundMoney = "На счете недостаточно средств";
    private const string CreditTransactionNotFoundErrorMessage = "Транзакция на списание не найдена";
    private const string DebitTransactionNotFoundErrorMessage = "Транзакция на пополнение не найдена";
    private const string CreditAccountNotFoundErrorMessage = "Счет на списание не найден";
    private const string DebitAccountNotFoundErrorMessage = "Счет на пополенение не найден";

    public async Task<Unit> Handle(ApplyTransactionPairCommand request, CancellationToken cancellationToken)
    {
        var transactions = (await transactionStorageService.GetTransactionsAsync([request.FirstTransactionId, request.SecondTransactionId], cancellationToken)).ToList();

        var credit = transactions.FirstOrDefault(x => x.Type == TransactionType.Credit)
            ?? throw new NotFoundException(CreditTransactionNotFoundErrorMessage);
        var debit = transactions.FirstOrDefault(x => x.Type == TransactionType.Debit)
            ?? throw new NotFoundException(DebitTransactionNotFoundErrorMessage);

        if (credit.BankAccountId == debit.CounterpartyBankAccountId &&
            credit.CounterpartyBankAccountId == debit.BankAccountId &&
            credit.CounterpartyBankAccountId is not null)
        {
            throw new UnprocessableException(InvalidTransactionLinkErrorMessage);
        }

        var accounts = (await accountStorageService.GetAccountsAsync(cancellationToken, credit.BankAccountId, credit.CounterpartyBankAccountId!.Value)).ToList();

        var creditAccount = accounts.SingleOrDefault(a => a.Id == credit.BankAccountId)
             ?? throw new NotFoundException(CreditAccountNotFoundErrorMessage);
        var debitAccount = accounts.SingleOrDefault(a => a.Id == debit.BankAccountId)
             ?? throw new NotFoundException(DebitAccountNotFoundErrorMessage);

        ProcessCredit(credit, creditAccount);
        ProcessDebit(debit, debitAccount);

        await transactionStorageService.ApplyTransactionsAsync([credit, debit], [creditAccount, debitAccount], cancellationToken);

        return Unit.Value;
    }

    private static void ProcessDebit(Transaction debit, Domains.Account debitAccount)
    {
        debitAccount.Balance += debit.Amount;
        debit.IsApply = true;
    }

    private static void ProcessCredit(Transaction credit, Domains.Account creditAccount)
    {
        if (creditAccount.Balance < credit.Amount) {
            throw new UnprocessableException(AccountNotFoundMoney);
        }

        creditAccount.Balance -= credit.Amount;
        credit.IsApply = true;
    }
}
