using AccountService.Api.ObjectStorage;
using FluentValidation;

namespace AccountService.Api.Features.Transactions.RegisterTransaction;

public class RegisterTransactionCommandValidator : AbstractValidator<RegisterTransactionCommand>
{
    public RegisterTransactionCommandValidator(ICurrencyHelper currencyHelper)
    {
        RuleFor(x => x.BankAccountId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(currencyHelper.IsValid);
        
        RuleFor(x => x.Type)
            .IsInEnum();

        RuleFor(x => x.Description)
            .NotEmpty();
    }
}