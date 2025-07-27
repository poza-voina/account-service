using AccountService.Api.Features.Statement.GetStatement;
using AccountService.Api.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Features.Statements;

[ApiController]
[Route("statements")]
public class StatementController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получает выписку по счету
    /// </summary>
    /// <returns></returns>
    /// <param name="accountId">Идентификатор счета</param>
    [HttpGet("{accountId:guid}")]
    public async Task<ActionResult<IEnumerable<AccountWithTransactionsViewModel>>> GetStatmentAsync([FromRoute] Guid accountId, [FromQuery] GetStatementQuery query)
    {
        query.AccountId = accountId;

        var result = await mediator.Send(query);

        return Ok(result);
    }
}
