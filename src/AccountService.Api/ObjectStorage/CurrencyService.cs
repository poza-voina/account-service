using AccountService.Api.ObjectStorage.Interfaces;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.ObjectStorage;

public class CurrencyService : ICurrencyService
{
    public Task<bool> IsCurrencySupportedByAccount(Models.Account account, string currency)
    {
        return Task.FromResult(account.Currency == currency);
    }
}