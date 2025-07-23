using AccountService.Api.Features.Account.CheckAccountExists;
using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Api.Features.Account.GetAccounts;
using AccountService.Api.Features.Account.RemoveAccount;
using AccountService.Api.Features.Account.UpdateAccount;
using AccountService.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Features.Account;

[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// Создает счет
    /// </summary>
    [HttpPost]
    public ActionResult<AccountViewModel> CreateAccount([FromBody] CreateAccountCommand command)
    {
        return Ok(null); // TODO: Сделать
    }

    /// <summary>
    /// Обновляет счет
    /// </summary>
    [HttpPut]
    public ActionResult<AccountViewModel> UpdateAccount([FromBody] UpdateAccountCommand command)
    {
        return Ok(null); // TODO: Сделать
    }

    /// <summary>
    /// Удаляет счет
    /// </summary>
    [HttpDelete("{id:Guid}")]
    public ActionResult<AccountViewModel> RemoveAccount(Guid id)
    {
        var command = new RemoveAccountCommand() { Id = id };
        return Ok(null); // TODO: Сделать
    }

    /// <summary>
    /// Получает список счетов
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<AccountViewModel>> GetAccounts([FromQuery] GetAccountsQuery query)
    {
        return Ok(null); // TODO: Сделать
    }

    /// <summary>
    /// Проверяет наличие счета
    /// </summary>
    [HttpGet("{id:long}/exists")]
    public ActionResult<AccountViewModel> CheckAccountExists([FromRoute] Guid id)
    {
        var query = new CheckAccountQuery() { Id = id };
        return Ok(null); // TODO: Сделать
    }
}
