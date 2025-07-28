using AccountService.Api.Features.Transactions.ExecuteTransaction;
using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.Features.Transactions.TransferTransaction;
using AccountService.Api.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Features.Transactions;

[ApiController]
[Route("transactions")]
public class TransactionController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Зарегистрировать транзакцию
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<TransactionViewModel>> RegisterTransactionAsync(RegisterTransactionCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Перенос между счетами
    /// </summary>
    [HttpPost("transfer")]
    public async Task<ActionResult<TransactionViewModel>> TransferTransaction(TransferTransactionCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Зарегистрировать и выполнить транзакцию
    /// </summary>
    [HttpPost("execute")]
    public async Task<ActionResult<TransactionViewModel>> TransferTransaction(ExecuteTransactionCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }
}