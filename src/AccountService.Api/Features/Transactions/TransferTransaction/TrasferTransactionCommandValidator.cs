using AccountService.Api.ObjectStorage;
using FluentValidation;

namespace AccountService.Api.Features.Transactions.TransferTransaction;

public class TrasferTransactionCommandValidator : AbstractValidator<TrasferTransactionCommand>
{
    public TrasferTransactionCommandValidator(ICurrencyHelper currencyHelper)
    {
        RuleFor(x => x.BankAccountId)
            .NotEmpty();

        RuleFor(x => x.CounterpartyBankAccountId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(currencyHelper.IsValid);

        RuleFor(x => x.Description)
            .NotEmpty();
    }
}
