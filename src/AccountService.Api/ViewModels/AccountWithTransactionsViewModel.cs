using JetBrains.Annotations;

namespace AccountService.Api.ViewModels;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class AccountWithTransactionsViewModel : AccountViewModel
{
    /// <summary>
    /// Коллекция транзакций
    /// </summary>
    public IEnumerable<TransactionViewModel> Transactions { get; set; } = [];
}
