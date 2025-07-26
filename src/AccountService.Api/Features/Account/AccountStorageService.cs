using AccountService.Api.Exceptions;
using AutoMapper;

namespace AccountService.Api.Features.Account;

public interface IAccountStorageService
{
    Task<Domains.Account> CreateAccountAsync(Domains.Account account, CancellationToken cancellationToken);
    Task<IEnumerable<Domains.Account>> GetAccountsAsync(CancellationToken cancellationToken);
    Task RemoveAccountAsync(Guid id, CancellationToken cancellationToken);
    Task<Domains.Account> GetAccountAsync(Guid id, CancellationToken cancellationToken);
    Task<Domains.Account> UpdateAccountAsync(Domains.Account account, CancellationToken cancellationToken);
    Task<bool> CheckExistsAsync(Guid id, CancellationToken cancellationToken);
}

public class AccountStorageService(ICollection<Domains.Account> accounts, IMapper mapper) : IAccountStorageService
{
    private const string AccountNotFoundErrorMessage = "Счет не найден";

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
}
