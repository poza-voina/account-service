using AccountService.Api.Features.Transactions.ExecuteTransaction;
using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.Features.Transactions.TransferTransaction;
using AccountService.Api.ViewModels.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionResponse = AccountService.Api.ViewModels.Result.MbResult<AccountService.Api.ViewModels.TransactionViewModel>;
using TransactionResponseError = AccountService.Api.ViewModels.Result.MbResult<object>;

namespace AccountService.Api.Features.Transactions;

[ApiController]
[Authorize]
[Route("transactions")]
public class TransactionController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Зарегистрировать транзакцию
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(TransactionResponse), 200)]
    [ProducesResponseType(typeof(TransactionResponseError), 400)]
    [ProducesResponseType(typeof(TransactionResponseError), 422)]
    [ProducesResponseType(typeof(TransactionResponseError), 401)]
    public async Task<ActionResult<TransactionResponse>> RegisterTransactionAsync(RegisterTransactionCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(MbResultFactory.WithSuccess(result));
    }

    /// <summary>
    /// Перенос между счетами
    /// </summary>
    [HttpPost("transfer")]
    [ProducesResponseType(typeof(TransactionResponseError), 400)]
    [ProducesResponseType(typeof(TransactionResponseError), 404)]
    [ProducesResponseType(typeof(TransactionResponseError), 422)]
    [ProducesResponseType(typeof(TransactionResponseError), 401)]
    public async Task<ActionResult<TransactionResponse>> TransferTransaction(TransferTransactionCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(MbResultFactory.WithSuccess(result));
    }

    /// <summary>
    /// Зарегистрировать и выполнить транзакцию
    /// </summary>
    [HttpPost("execute")]
    [ProducesResponseType(typeof(TransactionResponseError), 400)]
    [ProducesResponseType(typeof(TransactionResponseError), 404)]
    [ProducesResponseType(typeof(TransactionResponseError), 422)]
    [ProducesResponseType(typeof(TransactionResponseError), 401)]
    public async Task<ActionResult<TransactionResponse>> TransferTransaction(ExecuteTransactionCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(MbResultFactory.WithSuccess(result));
    }
}