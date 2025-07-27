using MediatR;

namespace AccountService.Api.Features.Account.RemoveAccount;

public class RemoveAccountCommand : IRequest<Unit>
{
    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public required Guid Id { get; set; }
}
