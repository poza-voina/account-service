using AccountService.Api.Features.Statements.GetStatement;
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
    /// <param name="query">Запрос на получение выписки</param>
    [HttpGet("{accountId:guid}")]
    public async Task<ActionResult<IEnumerable<AccountWithTransactionsViewModel>>> GetStatementAsync([FromRoute] Guid accountId, [FromQuery] GetStatementQuery query)
    {
        query.AccountId = accountId;

        var result = await mediator.Send(query);

        return Ok(result);
    }
}
