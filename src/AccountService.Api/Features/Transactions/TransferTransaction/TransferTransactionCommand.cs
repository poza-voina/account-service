using AccountService.Api.ViewModels;
using MediatR;

namespace AccountService.Api.Features.Transactions.TransferTransaction;

public class TransferTransactionCommand : IRequest<TransferTransactionViewModel>

{
    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public required Guid BankAccountId { get; set; }

    /// <summary>
    /// Версия счета
    /// </summary>
    public required uint BankAccountVersion { get; set; }

    /// <summary>
    /// Идентификатор счета контрагента
    /// </summary>
    public required Guid CounterpartyBankAccountId { get; set; }

    /// <summary>
    /// Версия счета контрагента
    /// </summary>
    public required uint CounterpartyBankAccountVersion { get; set; }

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