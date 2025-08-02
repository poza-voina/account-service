using AccountService.Api.Features.Statements.GetStatement;
using AccountService.Api.ViewModels.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StatementResponse = AccountService.Api.ViewModels.Result.MbResult<System.Collections.Generic.IEnumerable<AccountService.Api.ViewModels.AccountWithTransactionsViewModel>>;
using StatementResponseError = AccountService.Api.ViewModels.Result.MbResult<object>;

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
    [ProducesResponseType(typeof(StatementResponse), 200)]
    [ProducesResponseType(typeof(StatementResponseError), 404)]
    public async Task<ActionResult<StatementResponse>> GetStatementAsync([FromRoute] Guid accountId, [FromQuery] GetStatementQuery query)
    {
        query.AccountId = accountId;

        var result = await mediator.Send(query);

        return Ok(MbResultFactory.WithSuccess(result));
    }
}
