namespace AccountService.Api.Features.Account.Interfaces;

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

