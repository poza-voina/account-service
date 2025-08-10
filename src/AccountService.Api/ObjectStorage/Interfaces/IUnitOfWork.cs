using Microsoft.EntityFrameworkCore.Storage;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    public T GetRepository<T>() where T : class;
    public IExecutionStrategy CreateExecutionStrategy();
}