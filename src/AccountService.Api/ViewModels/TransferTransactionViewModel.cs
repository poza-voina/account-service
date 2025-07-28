namespace AccountService.Api.ViewModels;

public class TransferTransactionViewModel
{
    /// <summary>
    /// Транзакция на пополнение
    /// </summary>
    public required TransactionViewModel Debit { get; set; }

    /// <summary>
    /// Транзакция на списание
    /// </summary>
    public required TransactionViewModel Credit { get; set; }
}
