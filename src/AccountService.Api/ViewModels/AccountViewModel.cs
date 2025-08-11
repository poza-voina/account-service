using AccountService.Infrastructure.Enums;
using JetBrains.Annotations;

namespace AccountService.Api.ViewModels;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class AccountViewModel
{
    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Тип счета
    /// </summary>
    public AccountType Type { get; set; }

    /// <summary>
    /// Валюта
    /// </summary>
    public required string Currency { get; set; }

    /// <summary>
    /// Баланс счета
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Процентная ставка счета
    /// </summary>
    public decimal? InterestRate { get; set; }

    /// <summary>
    /// Дата открытия счета
    /// </summary>
    public DateTime OpeningDate { get; set; }

    /// <summary>
    /// Дата закрытия счета
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// Версия сущности
    /// </summary>
    public required uint Version { get; set; }
}