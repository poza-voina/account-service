using AccountService.Api.Features.Account.CheckAccountExists;
using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Api.Features.Account.GetAccounts;
using AccountService.Api.Features.Account.PatchAccount;
using AccountService.Api.Features.Account.RemoveAccount;
using AccountService.Api.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Features.Account;

[ApiController]
[Route("accounts")]
public class AccountController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создает счет
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AccountViewModel>> CreateAccount([FromBody] CreateAccountCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Обновляет счет
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<AccountViewModel>> UpdateAccount([FromBody] PatchAccountCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Удаляет счет
    /// </summary>
    /// <param name="id">Идентификатор счета</param>
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<AccountViewModel>> RemoveAccount(Guid id)
    {
        var command = new RemoveAccountCommand() { Id = id };

        var result = await mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Получает список счетов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountViewModel>>> GetAccounts([FromQuery] GetAccountsQuery query)
    {
        var result = await mediator.Send(query);

        return Ok(result);
    }

    /// <summary>
    /// Проверяет наличие счета
    /// </summary>
    /// <param name="id">Идентификатор счета</param>
    [HttpGet("{id:guid}/exists")]
    public async Task<ActionResult<AccountViewModel>> CheckAccountExists([FromRoute] Guid id)
    {
        var query = new CheckAccountQuery() { Id = id };

        var result = await mediator.Send(query);

        return Ok(result);
    }
}
