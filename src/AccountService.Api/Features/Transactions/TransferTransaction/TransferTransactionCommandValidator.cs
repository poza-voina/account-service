using AccountService.Api.ObjectStorage;
using FluentValidation;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Transactions.TransferTransaction;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class TransferTransactionCommandValidator : AbstractValidator<TransferTransactionCommand>
{
    public TransferTransactionCommandValidator(ICurrencyHelper currencyHelper)
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
