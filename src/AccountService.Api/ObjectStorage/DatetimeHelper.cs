using AccountService.Api.ObjectStorage.Interfaces;

namespace AccountService.Api.ObjectStorage;

public class DatetimeHelper : IDatetimeHelper
{
    private const string ErrorMessageFormat = "Start date ({0:yyyy-MM-dd HH:mm:ss}) cannot be greater than end date ({1:yyyy-MM-dd HH:mm:ss})";

    public (DateTime? start, DateTime? end) NormalizeDateRange(DateTime? start, DateTime? end)
    {
        var normalizedStart = start;
        var normalizedEnd = end;

        normalizedStart = normalizedStart?.Date;

        normalizedEnd = normalizedEnd?.Date.AddDays(1).AddTicks(-1);

        if (normalizedStart > normalizedEnd)
        {
            throw new ArgumentException(string.Format(ErrorMessageFormat, normalizedStart, normalizedEnd));
        }

        return (normalizedStart, normalizedEnd);
    }
}