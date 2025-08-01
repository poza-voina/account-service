namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IDatetimeHelper
{
    (DateTime? start, DateTime? end) NormalizeDateRange(DateTime? start, DateTime? end);
}
