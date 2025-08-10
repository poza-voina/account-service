using AccountService.Api.ViewModels;
using AccountService.Infrastructure.Enums;
using MediatR;

namespace AccountService.Api.Features.Account.CreateAccount;

public class CreateAccountCommand : IRequest<AccountViewModel>
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public required Guid OwnerId { get; set; }

    /// <summary>
    /// Тип Счета
    /// </summary>
    public required AccountType Type { get; set; }

    /// <summary>
    /// Валюта
    /// </summary>
    public required string Currency { get; set; }

    /// <summary>
    /// Процентная ставка
    /// </summary>
    public decimal? InterestRate { get; set; }

    /// <summary>
    /// Дата закрытия счета
    /// </summary>
    public DateTime? ClosingDate { get; set; }
}
