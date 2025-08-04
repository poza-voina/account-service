using AccountService.Api.Features.Account.CheckAccountExists;
using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Api.Features.Account.GetAccounts;
using AccountService.Api.Features.Account.PatchAccount;
using AccountService.Api.Features.Account.RemoveAccount;
using AccountService.Api.ViewModels.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AccountErrorResponse = AccountService.Api.ViewModels.Result.MbResult<object>;
using AccountResponse = AccountService.Api.ViewModels.Result.MbResult<AccountService.Api.ViewModels.AccountViewModel>;
using AccountsResponse = AccountService.Api.ViewModels.Result.MbResult<System.Collections.Generic.IEnumerable<AccountService.Api.ViewModels.AccountViewModel>>;

namespace AccountService.Api.Features.Account;

[ApiController]
[Authorize]
[Route("accounts")]
public class AccountController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создает счет
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AccountResponse), 200)]
    [ProducesResponseType(typeof(AccountErrorResponse), 422)]
    [ProducesResponseType(typeof(AccountErrorResponse), 400)]
    [ProducesResponseType(typeof(AccountErrorResponse), 401)]
    public async Task<ActionResult<AccountResponse>> CreateAccount([FromBody] CreateAccountCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(MbResultFactory.WithSuccess(result));
    }

    /// <summary>
    /// Обновляет счет
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(AccountErrorResponse), 422)]
    [ProducesResponseType(typeof(AccountErrorResponse), 400)]
    [ProducesResponseType(typeof(AccountErrorResponse), 404)]
    [ProducesResponseType(typeof(AccountErrorResponse), 401)]
    public async Task<ActionResult<AccountResponse>> UpdateAccount([FromBody] PatchAccountCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(MbResultFactory.WithSuccess(result));
    }

    /// <summary>
    /// Удаляет счет
    /// </summary>
    /// <param name="id">Идентификатор счета</param>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(AccountErrorResponse), 404)]
    [ProducesResponseType(typeof(AccountErrorResponse), 401)]
    public async Task<ActionResult<AccountResponse>> RemoveAccount(Guid id)
    {
        var command = new RemoveAccountCommand { Id = id };

        var result = await mediator.Send(command);

        return Ok(MbResultFactory.WithSuccess(result));
    }

    /// <summary>
    /// Получает список счетов
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(AccountErrorResponse), 401)]
    public async Task<ActionResult<AccountsResponse>> GetAccounts([FromQuery] GetAccountsQuery query)
    {
        var result = await mediator.Send(query);

        return Ok(MbResultFactory.WithSuccess(result));
    }

    /// <summary>
    /// Проверяет наличие счета
    /// </summary>
    /// <param name="id">Идентификатор счета</param>
    [HttpGet("{id:guid}/exists")]
    [ProducesResponseType(typeof(AccountErrorResponse), 200)]
    [ProducesResponseType(typeof(AccountErrorResponse), 404)]
    [ProducesResponseType(typeof(AccountErrorResponse), 401)]
    public async Task<ActionResult> CheckAccountExists([FromRoute] Guid id)
    {
        var query = new CheckAccountQuery { Id = id };

        var result = await mediator.Send(query);

        return Ok(MbResultFactory.WithSuccess(result));
    }
}
