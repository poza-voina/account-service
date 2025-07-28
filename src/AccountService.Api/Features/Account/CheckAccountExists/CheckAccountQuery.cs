using MediatR;

namespace AccountService.Api.Features.Account.CheckAccountExists;

public class CheckAccountQuery : IRequest<Unit>
{
    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public required Guid Id { get; set; }
}