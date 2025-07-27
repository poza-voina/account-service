using AccountService.Api.Domains;
using AccountService.Api.Exceptions;
using AutoMapper;

namespace AccountService.Api.Features.Transactions;

public interface ITransactionStorageService
{
    Task ApplyTransactionsAsync(IEnumerable<Transaction> applyTransactions, IEnumerable<Domains.Account> applyAccounts, CancellationToken cancellationToken);
    Task ApplyTransactionAsync(Transaction applyTransaction, Domains.Account applyAccount, CancellationToken cancellationToken);
    Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetTransactionsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    Task<Transaction> GetTransactionAsync(Guid id, CancellationToken cancellationToken);
}

public class TransactionStorageService(ICollection<Domains.Account> accounts, IMapper mapper) : ITransactionStorageService
{
    private const string AccountNotFoundErrorMessage = "Счет не найден";
    private const string TransactionNotFound = "Транзакция не найдена";

    public Task ApplyTransactionAsync(Transaction applyTransaction, Domains.Account applyAccount, CancellationToken cancellationToken)
    {
        var account = accounts.FirstOrDefault(x => x.Id == applyAccount.Id) ??
            throw new NotFoundException(AccountNotFoundErrorMessage);

        var transaction = account.Transactions.FirstOrDefault(x => x.Id == applyTransaction.Id) ??
            throw new NotFoundException(TransactionNotFound);

        mapper.Map(applyAccount, account);
        mapper.Map(applyTransaction, transaction);

        return Task.CompletedTask;
    }

    public async Task ApplyTransactionsAsync(IEnumerable<Transaction> applyTransactions, IEnumerable<Domains.Account> applyAccounts, CancellationToken cancellationToken)
    {
        var transactions = await GetTransactionsAsync(applyTransactions.Select(x => x.Id), cancellationToken);
        foreach (var item in transactions)
        {
            item.IsApply = true;
        }

        var foundAccounts = accounts.Where(x => applyAccounts.Select(x => x.Id).Contains(x.Id));

        var applyAccountsDict = applyAccounts.ToDictionary(a => a.Id);

        foreach (var account in accounts)
        {
            if (applyAccountsDict.TryGetValue(account.Id, out var updatedAccount))
            {
                mapper.Map(updatedAccount, account);
            }
        }
    }

    public Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        var account = accounts.FirstOrDefault(x => x.Id == transaction.BankAccountId) ?? throw new NotFoundException(AccountNotFoundErrorMessage);
        account.Transactions.Add(transaction);

        return Task.FromResult(transaction);
    }

    public Task<Transaction> GetTransactionAsync(Guid id, CancellationToken cancellationToken)
    {
        var transaction = accounts.SelectMany(x => x.Transactions).FirstOrDefault(x => x.Id == id) ??
            throw new NotFoundException(TransactionNotFound);

        return Task.FromResult(transaction);
    }

    public Task<IEnumerable<Transaction>> GetTransactionsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        var idSet = ids.ToHashSet();
        if (idSet.Count != ids.Count())
        {
            throw new InvalidOperationException("Входные данные содержат дублирующиеся идентификаторы транзакций."); // TODO: сделать
        }

        var result = accounts
            .SelectMany(x => x.Transactions)
            .Where(x => idSet.Contains(x.Id))
            .ToList();

        if (result.Count != idSet.Count)
        {
            throw new InvalidOperationException("Найдены не все транзакции"); // TODO: сделать
        }

        return Task.FromResult(result.AsEnumerable());
    }
}
