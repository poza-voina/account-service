using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Account.Interfaces;
using Models = AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Api.Features.Account;

public class AccountStorageService(IRepository<Models.Account> accountRepository, IMapper mapper) : IAccountStorageService
{
    private const string AccountNotFoundErrorMessage = "Счет не найден";

    public async Task<Models.Account> CreateAccountAsync(
        Models.Account account,
        CancellationToken cancellationToken) =>
        await accountRepository.AddAsync(mapper.Map<Models.Account>(account), cancellationToken);

    public async Task<IEnumerable<Models.Account>> GetAccountsAsync(CancellationToken cancellationToken) =>
        await accountRepository.GetAll().Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);

    public async Task RemoveAccountAsync(Guid id, CancellationToken cancellationToken)
    {
        var model = await accountRepository.FindOrDefaultAsync(id) ?? throw new NotFoundException(AccountNotFoundErrorMessage);

        model.IsDeleted = true;

        await accountRepository.UpdateAsync(model, cancellationToken);
    }

    public async Task<Models.Account> GetAccountAsync(
        Guid id,
        CancellationToken cancellationToken,
        Func<IQueryable<Models.Account>,
            IQueryable<Models.Account>>? configureQuery = null)
    {
        var query = accountRepository.GetAll();

        if (configureQuery is not null)
        {
            query = configureQuery(query);
        }

        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken) ?? throw new NotFoundException(AccountNotFoundErrorMessage);
    }

    public async Task<Models.Account> UpdateAccountAsync(
        Models.Account account,
        CancellationToken cancellationToken) =>
        await accountRepository.UpdateAsync(account, cancellationToken);

    public async Task<bool> CheckExistsAsync(
        Guid id,
        CancellationToken cancellationToken) =>
        await accountRepository.GetAll().AnyAsync(x => x.Id == id, cancellationToken: cancellationToken);
}
