using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AccountService.Api.ObjectStorage;

public class UnitOfWork(DbContext context, IServiceProvider serviceProvider) : IUnitOfWork, IDisposable
{
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync()
    {
        if (_transaction is { })
        {
            throw new InvalidOperationException("Transaction already started");
        }

        _transaction = await context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("No active transaction");
        }

        await context.SaveChangesAsync();
        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync()
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("No active transaction");
        }

        await _transaction.RollbackAsync();
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

    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();

    public IExecutionStrategy CreateExecutionStrategy() =>
        context.Database.CreateExecutionStrategy();

    public void Dispose()
    {
        // TODO: Замените UnitOfWork.Dispose() вызовом GC.SuppressFinalize(object). В результате для производных типов, использующих метод завершения, отпадет необходимость в повторной реализации "IDisposable" для вызова этого метода. Предложение студии
        _transaction?.Dispose();
        context?.Dispose();
    }
}