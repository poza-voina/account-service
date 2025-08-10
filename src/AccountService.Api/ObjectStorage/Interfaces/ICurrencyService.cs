using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface ICurrencyService
{
    public Task<bool> IsCurrencySupportedByAccount(Models.Account account, string currency);
}
