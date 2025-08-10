using AccountService.Api.ViewModels;
using AccountService.Infrastructure.Enums;
using JetBrains.Annotations;
using MediatR;

namespace AccountService.Api.Features.Transactions.ExecuteTransaction;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ExecuteTransactionCommand : IRequest<TransactionViewModel>
{
    /// <summary>
    /// Идентификатор счета
    /// </summary>
    public required Guid BankAccountId { get; set; }

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

    /// <summary>
    /// Версия сущности
    /// </summary>
    public uint BankAccountVersion { get; set; }
}
