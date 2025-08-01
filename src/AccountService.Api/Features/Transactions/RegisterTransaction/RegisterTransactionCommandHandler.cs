using AccountService.Api.Exceptions;
using AccountService.Api.Features.Account.CheckAccountExists;
using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.Features.Transactions.Interfaces;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ViewModels;
using AutoMapper;
using MediatR;

namespace AccountService.Api.Features.Transactions.RegisterTransaction;

public class RegisterTransactionCommandHandler(
    IAccountStorageService accountStorageService,
    ITransactionStorageService transactionStorageService,
    ICurrencyService currencyService,
    IMediator mediator,
    IMapper mapper)
    : IRequestHandler<RegisterTransactionCommand, TransactionViewModel>
{
    private const string CurrencyNotProcessedFormatErrorMessage = "Валюта {0} не поддерживается счетом";

    public async Task<TransactionViewModel> Handle(RegisterTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await accountStorageService.GetAccountAsync(request.BankAccountId, cancellationToken);

        if (request.CounterpartyBankAccountId.HasValue)
        {
            await mediator.Send(new CheckAccountQuery { Id = request.CounterpartyBankAccountId.Value }, cancellationToken);
        }

        if (!await currencyService.IsCurrencySupportedByAccount(account, request.Currency))
        {
            throw new UnprocessableException(string.Format(CurrencyNotProcessedFormatErrorMessage, request.Currency));
        }

        var transaction = mapper.Map<Domains.Transaction>(request);
        transaction.IsApply = false;
        transaction.Id = Guid.NewGuid();
        transaction.CreatedAt = DateTime.Now;

        var result = await transactionStorageService.CreateTransactionAsync(transaction, cancellationToken);

        return mapper.Map<TransactionViewModel>(result);
    }
}