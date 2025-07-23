namespace AccountService.Api.Features.Statements.GetStatements;

public class GetStatementsQuery
{
    public Guid? AccountId { get; set; }
    public Guid OwnerId { get; set; }
}
