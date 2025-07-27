namespace AccountService.Api.ViewModels;

public class AccountWithTransactionsViewModel : AccountViewModel
{
    /// <summary>
    /// Коллекция транзакций
    /// </summary>
    public IEnumerable<TransactionViewModel> Transactions { get; set; } = [];
}
