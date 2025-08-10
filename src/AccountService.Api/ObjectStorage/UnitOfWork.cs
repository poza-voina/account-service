using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Infrastructure;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace AccountService.Api.ObjectStorage;

public class UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider) : IUnitOfWork, IDisposable
{
    private IDbContextTransaction? _transaction;

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is { })
        {
            throw new InvalidOperationException("Transaction already started");
        }

        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        return _transaction;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("No active transaction");
        }

        await context.SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("No active transaction");
        }

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public T GetRepository<T>() where T : class
    {
        if (typeof(IDatabaseModel).IsAssignableFrom(typeof(T)))
        {
            var repoType = typeof(IRepository<>).MakeGenericType(typeof(T));
            return (T)serviceProvider.GetRequiredService(repoType);
        }

        return serviceProvider.GetRequiredService<T>();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
        await context.SaveChangesAsync(cancellationToken);

    public IExecutionStrategy CreateExecutionStrategy() =>
        context.Database.CreateExecutionStrategy();

    public void Dispose()
    {
        // TODO: Замените UnitOfWork.Dispose() вызовом GC.SuppressFinalize(object). В результате для производных типов, использующих метод завершения, отпадет необходимость в повторной реализации "IDisposable" для вызова этого метода. Предложение студии
        _transaction?.Dispose();
        context?.Dispose();
    }
}