namespace AccountService.Api.ViewModels;

public class TransferTransactionViewModel
{
    public required TransactionViewModel Debit { get; set; }
    public required TransactionViewModel Credit { get; set; }
}
