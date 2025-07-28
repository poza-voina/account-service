using AccountService.Api.Domains.Enums;
using JetBrains.Annotations;

namespace AccountService.Api.ViewModels;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class TransactionViewModel
{
    /// <summary>
    /// Идентификатор транзакции
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public required Guid BankAccountId { get; set; }

    /// <summary>
    /// Идентификатор счета контрагента
    /// </summary>
    public Guid? CounterpartyBankAccountId { get; set; }

    /// <summary>
    /// Количество денег
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// Валюта
    /// </summary>
    public required string Currency { get; set; }

    /// <summary>
    /// Тип транзакции
    /// </summary>
    public required TransactionType Type { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Дата создания транзакции
    /// </summary>
    public required DateTime CreatedAt { get; set; }
}
