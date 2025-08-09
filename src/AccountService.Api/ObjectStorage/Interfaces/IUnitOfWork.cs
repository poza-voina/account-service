using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task SaveChangesAsync();
    public T GetRepository<T>() where T : class;
}