using MediatR;

namespace AccountService.Api.Features.Account.CheckAccountExists;

public class CheckAccountQuery : IRequest<Unit>
{
    public required Guid Id { get; set; }
}