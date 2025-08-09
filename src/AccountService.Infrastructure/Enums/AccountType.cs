namespace AccountService.Infrastructure.Enums;

/// <summary>
/// Тип банковского счета
/// </summary>
public enum AccountType
{
    /// <summary>
    /// Дебитовый счет
    /// </summary>
    Checking,

    /// <summary>
    /// Депозитный счет
    /// </summary>
    Deposit,

    /// <summary>
    /// Кредитный счет
    /// </summary>
    Credit
}