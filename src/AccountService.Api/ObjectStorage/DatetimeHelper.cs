namespace AccountService.Api.ObjectStorage;

public interface IDatetimeHelper
{
    (DateTime? start, DateTime? end) NormalizeDateRange(DateTime? start, DateTime? end);
}

public class DatetimeHelper : IDatetimeHelper
{
    private const string ErrorMessageFormat = "Start date ({0:yyyy-MM-dd HH:mm:ss}) cannot be greater than end date ({1:yyyy-MM-dd HH:mm:ss})";

    public (DateTime? start, DateTime? end) NormalizeDateRange(DateTime? start, DateTime? end)
    {
        DateTime? normalizedStart = start;
        DateTime? normalizedEnd = end;

        if (normalizedStart.HasValue)
        {
            normalizedStart = normalizedStart.Value.Date;
        }

        if (normalizedEnd.HasValue)
        {
            normalizedEnd = normalizedEnd.Value.Date.AddDays(1).AddTicks(-1);
        }

        if (normalizedStart.HasValue && normalizedEnd.HasValue && normalizedStart > normalizedEnd)
        {
            throw new ArgumentException(string.Format(ErrorMessageFormat, normalizedStart, normalizedEnd));
        }

        return (normalizedStart, normalizedEnd);
    }
}