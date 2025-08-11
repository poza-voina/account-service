using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Infrastructure;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace AccountService.Api.ObjectStorage;

public class UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider) : IUnitOfWork, IDisposable
{
    private IDbContextTransaction? _transaction;

    public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
    {
        if (_transaction is not null)
        {
            throw new InvalidOperationException("Transaction already started");
        }

        var connection = context.Database.GetDbConnection();

        if (connection.State is not ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var dbTransaction = await connection.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        _transaction = await context.Database.UseTransactionAsync(dbTransaction, cancellationToken)
            ?? throw new InvalidOperationException("Failed to bind transaction");

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
        if (!typeof(IDatabaseModel).IsAssignableFrom(typeof(T)))
        {
            return serviceProvider.GetRequiredService<T>();
        }

        var repoType = typeof(IRepository<>).MakeGenericType(typeof(T));
        
        return (T)serviceProvider.GetRequiredService(repoType);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        context.Dispose();
    }
}