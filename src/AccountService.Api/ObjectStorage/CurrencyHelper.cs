using System.Globalization;

namespace AccountService.Api.ObjectStorage;

public interface ICurrencyHelper
{
    bool IsValid(string currency);
}

public class CurrencyHelper : ICurrencyHelper
{
    HashSet<string> ISOCurrencyCodes { get; } =
        CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(x => new RegionInfo(x.Name))
            .Select(x => x.ISOCurrencySymbol)
            .ToHashSet();

    public bool IsValid(string currency)
    {
        return ISOCurrencyCodes.Contains(currency);
    }
}
