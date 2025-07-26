using MediatR;

namespace AccountService.Api.Features.Account.RemoveAccount;

public class RemoveAccountCommand : IRequest<Unit>
{
    public required Guid Id { get; set; }
}
