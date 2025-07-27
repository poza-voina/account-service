namespace AccountService.Api.ObjectStorage;

public interface ICurrencyService
{
    public Task<bool> IsCurrencySupportedByAccount(Domains.Account account, string currency);
}

public class CurrencyService : ICurrencyService
{
    public Task<bool> IsCurrencySupportedByAccount(Domains.Account account, string currency)
    {
        return Task.FromResult(account.Currency == currency);
    }
}