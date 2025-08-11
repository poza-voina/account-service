namespace AccountService.Infrastructure.Enums;

/// <summary>
/// Тип транзакции
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Транзакция списания
    /// </summary>
    Credit,

    /// <summary>
    /// Транзакция пополнения
    /// </summary>
    Debit
}
