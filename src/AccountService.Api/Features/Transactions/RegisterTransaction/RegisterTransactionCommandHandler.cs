using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Account.CheckAccountExists;
using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.Features.Transactions.Interfaces;
using AccountService.Api.ObjectStorage;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ViewModels;
using AccountService.Infrastructure.Enums;
using AutoMapper;
using MediatR;
using System.Data;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Features.Transactions.RegisterTransaction;

public class RegisterTransactionCommandHandler(
    IServiceProvider provider,
    IAccountStorageService accountStorageService,
    ITransactionStorageService transactionStorageService,
    ICurrencyService currencyService,
    IMediator mediator,
    IMapper mapper)
    : UnitHandlerBase<RegisterTransactionCommand, TransactionViewModel>(provider),
    IRequestHandler<RegisterTransactionCommand, TransactionViewModel>
{
    private const string CurrencyNotProcessedFormatErrorMessage = "Валюта {0} не поддерживается счетом";

    protected override async Task<TransactionViewModel> ExecuteTransactionBodyAsync(RegisterTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await accountStorageService.GetAccountAsync(request.BankAccountId, cancellationToken);

        if (account.IsFrozen && request.Type == TransactionType.Credit)
        {
            throw new ConflictException("Аккаунт заморожен");
        }

        if (request.CounterpartyBankAccountId.HasValue)
        {
            await mediator.Send(new CheckAccountQuery { Id = request.CounterpartyBankAccountId.Value }, cancellationToken);
        }

        if (!await currencyService.IsCurrencySupportedByAccount(account, request.Currency))
        {
            throw new UnprocessableException(string.Format(CurrencyNotProcessedFormatErrorMessage, request.Currency));
        }

        var transaction = mapper.Map<Models.Transaction>(request);
        transaction.IsApply = false;
        transaction.Id = Guid.NewGuid();

        var result = await transactionStorageService.CreateTransactionAsync(transaction, cancellationToken);

        return mapper.Map<TransactionViewModel>(result);
    }
}