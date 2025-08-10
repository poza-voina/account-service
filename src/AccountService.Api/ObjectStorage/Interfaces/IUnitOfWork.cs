using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    public T GetRepository<T>() where T : class;
    public IExecutionStrategy CreateExecutionStrategy();
}