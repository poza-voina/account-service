using AccountService.Api.Features.Statements.GetStatements;
using AccountService.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Features.Statements;

[ApiController]
[Route("statements")]
public class StatementController : ControllerBase
{
    /// <summary>
    /// Получает выписки по всем счетам c фильтрами
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<IEnumerable<AccountWithTransactionsViewModel>> GetStatments([FromQuery] GetStatementsQuery query)
    {
        return Ok(null); // TODO: сделать
    }
}
