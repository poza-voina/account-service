using AccountService.Api.Domains;

namespace AccountService.Api.Features.Transactions.Interfaces;

public interface ITransactionStorageService
{
    Task ApplyTransactionsAsync(IEnumerable<Transaction> applyTransactions, IEnumerable<Domains.Account> applyAccounts, CancellationToken cancellationToken);
    Task ApplyTransactionAsync(Transaction applyTransaction, Domains.Account applyAccount, CancellationToken cancellationToken);
    Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetTransactionsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    Task<Transaction> GetTransactionAsync(Guid id, CancellationToken cancellationToken);
}