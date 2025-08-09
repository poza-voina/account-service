using AccountService.Api.Domains.Enums;
using AccountService.Api.ObjectStorage.Interfaces;
using FluentValidation;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Account.CreateAccount;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator(ICurrencyHelper currencyHelper)
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty()
            .WithMessage("Идентификатор клиента не может быть пустым");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Несуществующий тип счета");
        
        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(currencyHelper.IsValid)
            .WithMessage("Валюта должна быть написана в формате ISO 4217");

        RuleFor(x => x.InterestRate)
            .Null()
            .When(x => x.Type == AccountType.Checking)
            .WithMessage("Ключевая ставка должна быть пустой, если тип счета Checking");
        RuleFor(x => x.InterestRate)
            .NotEmpty()
            .When(x => x.Type is AccountType.Deposit or AccountType.Credit)
            .WithMessage("Ключевая ставка может быть пустой только при типе счета Checking");

        RuleFor(x => x.ClosingDate)
            .GreaterThanOrEqualTo(GetCurrentDatetime())
            .WithMessage("Дата закрытия счета должна быть в будущем");
    }

    private static DateTime GetCurrentDatetime()
    {
        var now = DateTime.UtcNow;
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc);
    }
}
