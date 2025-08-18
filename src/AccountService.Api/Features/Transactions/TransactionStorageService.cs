using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Transactions.Interfaces;
using AccountService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Features.Transactions;

public class TransactionStorageService(IRepository<Models.Account> accountRepository, IRepository<Models.Transaction> transactionRepository) : ITransactionStorageService
{
    private const string TransactionNotFound = "Транзакция не найдена";
    private const string TransactionIdsDuplicatesErrorMessage = "Входные данные содержат дублирующиеся идентификаторы транзакций.";
    private const string TransactionNotAllFoundsErrorMessage = "Найдены не все транзакции";

    public async Task ApplyTransactionAsync(
        Models.Transaction transaction, Models.Account account,
        CancellationToken cancellationToken)
    {
        transaction.IsApply = true;
        await transactionRepository.UpdateAsync(transaction, cancellationToken);
        await accountRepository.UpdateAsync(account, cancellationToken);
    }

    public async Task ApplyTransactionsAsync(
        IEnumerable<Models.Transaction> transactions,
        IEnumerable<Models.Account> accounts,
        CancellationToken cancellationToken)
    {
        var models = transactions.ToList();
        foreach (var transaction in models)
        {
            transaction.IsApply = true;
        }

        await transactionRepository.UpdateRangeAsync(models);
        await accountRepository.UpdateRangeAsync(accounts);
    }

    public async Task<Models.Transaction> CreateTransactionAsync(
        Models.Transaction transaction,
        CancellationToken cancellationToken)
    {
        var isAccountExists = await accountRepository.GetAll()
            .AnyAsync(x => x.Id == transaction.BankAccountId, cancellationToken);

        if (!isAccountExists)
        {
            throw new NotFoundException($"Не удалось найти счет с id = {transaction.BankAccountId}");
        }

        transaction = await transactionRepository.AddAsync(transaction, cancellationToken);

        return transaction;
    }

    public async Task<Models.Transaction> GetTransactionAsync(
        Guid id,
        CancellationToken cancellationToken,
        Func<IQueryable<Models.Transaction>,
            IQueryable<Models.Transaction>>? configureQuery = null)
    {
        var query = transactionRepository.GetAll();

        if (configureQuery is not null)
        {
            query = configureQuery(query);
        }

        var transaction = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken) ??
            throw new NotFoundException(TransactionNotFound);

        return transaction;
    }

    public async Task<IEnumerable<Models.Transaction>> GetTransactionsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken,
        Func<IQueryable<Models.Transaction>,
            IQueryable<Models.Transaction>>? configureQuery = null)
    {
        ids = [.. ids];
        var guids = ids.ToList();
        var prevIdsLength = guids.Count;
        ids = guids.ToHashSet();

        if (ids.Count() != prevIdsLength)
        {
            throw new InvalidOperationException(TransactionIdsDuplicatesErrorMessage);
        }

        var query = transactionRepository.GetAll();

        if (configureQuery is not null)
        {
            query = configureQuery(query);
        }

        var transactions = await query.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        if (transactions.Count != ids.Count())
        {
            throw new InvalidOperationException(TransactionNotAllFoundsErrorMessage);
        }

        return transactions;
    }
}
