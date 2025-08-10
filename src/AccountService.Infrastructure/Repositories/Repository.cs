using AccountService.Abstractions.Exceptions;
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

    public async Task AddRangeAsync(
        IEnumerable<TModel> entities,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await context.AddRangeAsync(entities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        foreach (TModel entity in entities)
        {
            context.Entry(entity).State = EntityState.Detached;
        }
    }

    public async Task AddRangeAsync(params TModel[] entities) =>
        await AddRangeAsync(entities.AsEnumerable());

    public Task<bool> CanConnectToDbAsync(CancellationToken cancellationToken = default) =>
        context.Database.CanConnectAsync(cancellationToken);

    public async Task DeleteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindOrDefaultAsync(id) ?? throw new NotFoundException($"Сущность с id = {id} не найдена");

        await DeleteAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindOrDefaultAsync(id) ?? throw new NotFoundException($"Сущность с id = {id} не найдена");
        await DeleteAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(
        TModel entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        context.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        context.Entry(entity).State = EntityState.Detached;
    }

    public async Task DeleteRangeAsync(params TModel[] entities)
    {
        if (entities.Length > 0)
        {
            foreach (var entity in entities)
            {
                context.Entry(entity).State = EntityState.Deleted;
            }

            context.RemoveRange(entities);
            await context.SaveChangesAsync();
        }
    }

    public async Task<int> ExecuteSqlAsync(
        FormattableString sqlQuery,
        CancellationToken cancellationToken = default) =>
        await context.Database.ExecuteSqlAsync(sqlQuery, cancellationToken);

    public async Task<TModel?> FindOrDefaultAsync(params object[] objects)
    {
        ArgumentNullException.ThrowIfNull(objects);
        var entity = await context.FindAsync<TModel>(objects);

        if (entity is { })
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
        foreach (TModel entity in entities)
        {
            await UpdateAsync(entity);
        }
    }
}