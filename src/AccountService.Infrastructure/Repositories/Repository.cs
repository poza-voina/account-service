using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Repositories;

public class Repository<TModel>(ApplicationDbContext context) : IRepository<TModel> where TModel : class, IDatabaseModel
{
    public async Task<TModel> AddAsync(TModel entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await context.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        context.Entry(entity).State = EntityState.Detached;

        return entity;
    }

    public async Task<int> ExecuteSqlAsync(
        FormattableString sqlQuery,
        CancellationToken cancellationToken = default) =>
        await context.Database.ExecuteSqlAsync(sqlQuery, cancellationToken);

    public async Task<TModel?> FindOrDefaultAsync(params object[] objects)
    {
        ArgumentNullException.ThrowIfNull(objects);
        var entity = await context.FindAsync<TModel>(objects);

        if (entity is not null)
        {
            context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public IQueryable<TModel> GetAll() =>
        context
            .Set<TModel>()
            .AsNoTracking();

    public async Task<TModel> UpdateAsync(
        TModel entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (context.Entry(entity).State is EntityState.Detached)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        context.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        context.Entry(entity).State = EntityState.Detached;

        return entity;
    }

    public async Task UpdateRangeAsync(IEnumerable<TModel> entities)
    {
        foreach (var entity in entities)
{
            await UpdateAsync(entity);
        }
    }
}
