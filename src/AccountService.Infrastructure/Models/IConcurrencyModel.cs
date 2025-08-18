namespace AccountService.Infrastructure.Models;

public interface IConcurrencyModel
{
    uint Version { get; set; }
}
