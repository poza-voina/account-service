using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.Features.Transactions.TransferTransaction;
using AccountService.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Features.Transactions;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    /// <summary>
    /// Зарегистрировать транзакцию
    /// </summary>
    [HttpPost("register")]
    public ActionResult<TransactionViewModel> RegisterTransaction(RegisterTransactionCommand command)
    {
        return Ok(null); // TODO: сделать
    }

    /// <summary>
    /// Перенос между счетами
    /// </summary>
    [HttpPost("transfer")]
    public ActionResult<TransactionViewModel> TransferTransaction(TrasferTransactionCommand command)
    {
        return Ok(null); // TODO: сделать
    }
}