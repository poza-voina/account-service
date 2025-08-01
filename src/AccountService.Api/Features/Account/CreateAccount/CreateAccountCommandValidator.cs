using AccountService.Api.Domains.Enums;
using AccountService.Api.ObjectStorage;
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
            .WithMessage("������������� ������� �� ����� ���� ������");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("�������������� ��� �����");
        
        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(currencyHelper.IsValid)
            .WithMessage("������ ������ ���� �������� � ������� ISO 4217");

        RuleFor(x => x.InterestRate)
            .Null()
            .When(x => x.Type == AccountType.Checking)
            .WithMessage("�������� ������ ������ ���� ������, ���� ��� ����� Checking");
        RuleFor(x => x.InterestRate)
            .NotEmpty()
            .When(x => x.Type is AccountType.Deposit or AccountType.Credit)
            .WithMessage("�������� ������ ����� ���� ������ ������ ��� ���� ����� Checking");

        RuleFor(x => x.ClosingDate)
            .GreaterThanOrEqualTo(GetCurrentDatetime())
            .WithMessage("���� �������� ����� ������ ���� � �������");
    }

    private static DateTime GetCurrentDatetime()
    {
        var now = DateTime.UtcNow;
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc);
    }
}
