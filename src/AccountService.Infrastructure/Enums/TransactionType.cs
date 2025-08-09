namespace AccountService.Infrastructure.Enums;

/// <summary>
/// Тип транзакции
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Тразакция списания
    /// </summary>
    Credit,

    /// <summary>
    /// Транзакция пополенения
    /// </summary>
    Debit
}
