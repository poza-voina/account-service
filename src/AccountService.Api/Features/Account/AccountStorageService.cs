using AccountService.Api.Domains;
using AccountService.Api.Exceptions;
using AutoMapper;

namespace AccountService.Api.Features.Account;

public interface IAccountStorageService
{
    Task<Domains.Account> CreateAccountAsync(Domains.Account account, CancellationToken cancellationToken);
    Task<IEnumerable<Domains.Account>> GetAccountsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Domains.Account>> GetAccountsAsync(CancellationToken cancellationToken, params Guid[] ids);
    Task RemoveAccountAsync(Guid id, CancellationToken cancellationToken);
    Task<Domains.Account> GetAccountAsync(Guid id, CancellationToken cancellationToken);
    Task<Domains.Account> UpdateAccountAsync(Domains.Account account, CancellationToken cancellationToken);
    Task<bool> CheckExistsAsync(Guid id, CancellationToken cancellationToken);
}

public class AccountStorageService(ICollection<Domains.Account> accounts, IMapper mapper) : IAccountStorageService
{
    private const string AccountNotFoundErrorMessage = "Счет не найден";
    private const string AccountsNotFoundErrorMessage = "Некоторые аккаунты не найдены";
    private const string DuplicateAccountIdsErrorMessage = "Список идентификаторов счетов содержит дубликаты";

    public Task<Domains.Account> CreateAccountAsync(Domains.Account account, CancellationToken cancellationToken)
    {
        account.OpeningDate = DateTime.UtcNow;
        accounts.Add(account);
        return Task.FromResult(account);
    }

    public Task<IEnumerable<Domains.Account>> GetAccountsAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(accounts.Where(x => x.IsDeleted == false).AsEnumerable());
    }

    public Task RemoveAccountAsync(Guid id, CancellationToken cancellationToken)
    {
        var account = accounts.FirstOrDefault(x => x.Id == id) ?? throw new NotFoundException(AccountNotFoundErrorMessage);
        
        account.IsDeleted = true;

        return Task.CompletedTask;
    }

    public Task<Domains.Account> GetAccountAsync(Guid id, CancellationToken cancellationToken)
    {
        var account = accounts.FirstOrDefault(x => x.Id == id) ?? throw new NotFoundException(AccountNotFoundErrorMessage);

        return Task.FromResult(account);
    }

    public Task<Domains.Account> UpdateAccountAsync(Domains.Account account, CancellationToken cancellationToken)
    {
        var prevAccount = accounts.FirstOrDefault(x => x.Id == account.Id) ?? throw new NotFoundException(AccountNotFoundErrorMessage);

        mapper.Map(account, prevAccount);

        return Task.FromResult(prevAccount);
    }
    
    public Task<bool> CheckExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(accounts.Any(x => x.Id == id));
    }

    public Task<IEnumerable<Domains.Account>> GetAccountsAsync(CancellationToken cancellationToken, params Guid[] ids)
    {
        var idSet = ids.ToHashSet();
        if (ids.Length != idSet.Count)
        {
            throw new UnprocessableException(DuplicateAccountIdsErrorMessage);
        }

        var result = accounts
            .Where(account => idSet.Contains(account.Id))
            .AsEnumerable();

        if (result.Count() != idSet.Count)
        {
            throw new NotFoundException(AccountsNotFoundErrorMessage);
        }

        return Task.FromResult(result);
    }
}
