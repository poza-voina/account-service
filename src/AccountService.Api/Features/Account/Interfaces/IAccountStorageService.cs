using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Features.Account.Interfaces;

public interface IAccountStorageService
{
    Task<Models.Account> CreateAccountAsync(
        Models.Account account,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<Models.Account>> GetAccountsAsync(CancellationToken cancellationToken);
    
    Task<IEnumerable<Models.Account>> GetAccountsAsync(
        CancellationToken cancellationToken,
        params Guid[] ids);
    
    Task RemoveAccountAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task<Models.Account> GetAccountAsync(
        Guid id,
        CancellationToken cancellationToken,
        Func<IQueryable<Models.Account>, IQueryable<Models.Account>>? configureQuery = null);
    
    Task<Models.Account> UpdateAccountAsync(
        Models.Account account,
        CancellationToken cancellationToken);
    
    Task<bool> CheckExistsAsync(
        Guid id,
        CancellationToken cancellationToken);
}

