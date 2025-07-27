using AccountService.Api.Domains.Enums;
using AccountService.Api.ViewModels;
using MediatR;

namespace AccountService.Api.Features.Transactions.TransferTransaction;

public class TrasferTransactionCommand : IRequest<TransferTransactionViewModel>
{
    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public required Guid BankAccountId { get; set; }

    /// <summary>
    /// Идентификато счета контрагента
    /// </summary>
    public required Guid CounterpartyBankAccountId { get; set; }

    /// <summary>
    /// Количество денег
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// Валюта
    /// </summary>
    public required string Currency { get; set; }

    /// <summary>
    /// Описание транзакции
    /// </summary>
    public required string Description { get; set; }
}