using JetBrains.Annotations;
using MediatR;

namespace AccountService.Api.Features.Account.RemoveAccount;

public class RemoveAccountCommand : IRequest<Unit>
{
    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Версия сущности
    /// </summary>
    [UsedImplicitly]
    public uint Version { get; set; }
}
