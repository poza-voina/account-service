using AccountService.Api.Domains.Enums;
using AccountService.Api.ViewModels;
using MediatR;

namespace AccountService.Api.Features.Transactions.RegisterTransaction;

public class RegisterTransactionCommand : IRequest<TransactionViewModel>
{
    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public required Guid BankAccountId { get; set; }

    /// <summary>
    /// Идентификатор счета контрагента
    /// </summary>
    public Guid? CounterpartyBankAccountId { get; set; }

    /// <summary>
    /// Количество денег
    /// </summary>
    public required decimal Amount { get; set; }
    
    /// <summary>
    /// Валюта
    /// </summary>
    public required string Currency { get; set; }

    /// <summary>
    /// Тип транзакции
    /// </summary>
    public required TransactionType Type { get; set; }

    /// <summary>
    /// Описание транзакции
    /// </summary>
    public required string Description { get; set; }
}
