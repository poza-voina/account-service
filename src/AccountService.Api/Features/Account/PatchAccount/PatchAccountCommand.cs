using AccountService.Api.ViewModels;
using MediatR;

namespace AccountService.Api.Features.Account.PatchAccount;

public class PatchAccountCommand : IRequest<AccountViewModel>
{
    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Дата закрытия счета
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// Процентная ставка
    /// </summary>
    public decimal? InterestRate { get; set; }

    /// <summary>
    /// Версия сущности
    /// </summary>
    public uint Version { get; set; }
}