using AccountService.Api.ViewModels;
using MediatR;

namespace AccountService.Api.Features.Statement.GetStatement;

public class GetStatementQuery : IRequest<AccountWithTransactionsViewModel>
{
    public required Guid OwnerId { get; set; }
    public required Guid AccountId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
}
