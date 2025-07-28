using AccountService.Api.ViewModels;
using MediatR;

namespace AccountService.Api.Features.Statements.GetStatement;

public class GetStatementQuery : IRequest<AccountWithTransactionsViewModel>
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public required Guid OwnerId { get; set; }

    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public required Guid AccountId { get; set; }

    /// <summary>
    /// Начала диапозона выписки
    /// </summary>
    public DateTime? StartDateTime { get; set; }

    /// <summary>
    /// Конец диапозона выписки
    /// </summary>
    public DateTime? EndDateTime { get; set; }
}
