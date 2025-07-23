namespace AccountService.Api.ViewModels;

public class AccountWithTransactionsViewModel : AccountViewModel
{
    public IEnumerable<TransactionViewModel> Transactions { get; set; } = [];
}
