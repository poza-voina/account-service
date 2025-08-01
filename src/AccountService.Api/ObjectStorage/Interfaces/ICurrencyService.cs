namespace AccountService.Api.ObjectStorage.Interfaces;

public interface ICurrencyService
{
    public Task<bool> IsCurrencySupportedByAccount(Domains.Account account, string currency);
}
