using AccountService.Api.Domains.Enums;
using AccountService.Api.ObjectStorage;
using FluentValidation;

namespace AccountService.Api.Features.Account.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator(ICurrencyHelper currencyHelper)
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty();

        RuleFor(x => x.Type)
            .IsInEnum();
        
        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(currencyHelper.IsValid);

        RuleFor(x => x.InterestRate).Null()
            .When(x => x.Type == AccountType.Checking);
        RuleFor(x => x.InterestRate).NotEmpty()
            .When(x => x.Type == AccountType.Deposit || x.Type == AccountType.Credit);

        RuleFor(x => x.ClosingDate).GreaterThanOrEqualTo(GetCurrentDatetime());
    }

    private DateTime GetCurrentDatetime()
    {
        var now = DateTime.UtcNow;
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc);
    }
}
