using AccountService.Api.ObjectStorage.Interfaces;
using System.Globalization;

namespace AccountService.Api.ObjectStorage;

public class CurrencyHelper : ICurrencyHelper
{
    private HashSet<string> IsoCurrencyCodes { get; } =
        CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(x => new RegionInfo(x.Name))
            .Select(x => x.ISOCurrencySymbol)
            .ToHashSet();

    public bool IsValid(string currency)
    {
        return IsoCurrencyCodes.Contains(currency);
    }
}
