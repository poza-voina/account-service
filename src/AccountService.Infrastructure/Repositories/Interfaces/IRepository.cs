using AccountService.Infrastructure.Models;

namespace AccountService.Infrastructure.Repositories.Interfaces;

public interface IRepository<TModel> where TModel : class, IDatabaseModel
{
    Task<TModel> AddAsync(
        TModel entity,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<TModel> entities,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(params TModel[] entities);

    Task<bool> CanConnectToDbAsync(CancellationToken cancellationToken = default);

    Task DeleteAsync(
        long id,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        TModel entity,
        CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(params TModel[] entities);

    Task<TModel?> FindOrDefaultAsync(params object[] objects);

    IQueryable<TModel> GetAll();

    Task<TModel> UpdateAsync(
        TModel entity,
        CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TModel> entities);

    Task<int> ExecuteSqlAsync(
        FormattableString sqlQuery,
        CancellationToken cancellationToken = default);
}