using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Features.Transactions.Interfaces;

public interface ITransactionStorageService
{
    Task ApplyTransactionsAsync(
        IEnumerable<Models.Transaction> transactions,
        IEnumerable<Models.Account> accounts,
        CancellationToken cancellationToken);

    Task ApplyTransactionAsync(
        Models.Transaction transaction,
        Models.Account account,
        CancellationToken cancellationToken);

    Task<Models.Transaction> CreateTransactionAsync(
        Models.Transaction transaction,
        CancellationToken cancellationToken);

    Task<IEnumerable<Models.Transaction>> GetTransactionsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken);

    Task<Models.Transaction> GetTransactionAsync(
        Guid id,
        CancellationToken cancellationToken);
}