using AccountService.Api.ObjectStorage.Interfaces;

namespace AccountService.Api.ObjectStorage;

public class CurrencyService : ICurrencyService
{
    public Task<bool> IsCurrencySupportedByAccount(Domains.Account account, string currency)
    {
        return Task.FromResult(account.Currency == currency);
    }
}