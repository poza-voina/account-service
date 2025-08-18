using AccountService.Infrastructure.Models;

namespace AccountService.Infrastructure.Repositories.Interfaces;

public interface IRepository<TModel> where TModel : class, IDatabaseModel
{
    Task<TModel> AddAsync(
        TModel entity,
        CancellationToken cancellationToken = default);

    Task<TModel?> FindOrDefaultAsync(params object[] objects);

    IQueryable<TModel> GetAll();

    Task<TModel> UpdateAsync(
        TModel entity,
        CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TModel> entities);

    Task<int> ExecuteSqlAsync(
        FormattableString sqlQuery,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TModel>> AddRangeAsync(
        IEnumerable<TModel> entities,
        CancellationToken cancellationToken = default);

    Task<bool> CanConnectToDb();
}