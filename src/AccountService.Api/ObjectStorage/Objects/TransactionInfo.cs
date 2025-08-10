using AccountService.Infrastructure.Enums;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.ObjectStorage.Objects;

public class TransactionInfo
{
    public Guid TransactionId { get; set; }
    public uint AccountVersion { get; set; }

    private Models.Transaction? _transaction;

    public Models.Transaction Transaction
    {
        get => _transaction ?? throw new InvalidOperationException("Транзакция не была инициализирована");
        private set => _transaction = value;
    }

    public TransactionInfo WithTransaction(Models.Transaction transaction)
    {
        Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        TransactionId = transaction.Id;
        return this;
    }

    public TransactionType GetTransactionType() =>
        Transaction.Type;
}